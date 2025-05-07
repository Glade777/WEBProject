using Gimify.Entities;
using Gimify.DAL.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Gimify.BLL.Services
{
    public interface IPostService
    {
        void CreatePost(Posts post);
        void UpdatePost(Posts post);
        void DeletePost(int id);
        Posts? GetById(int id);
        List<Posts> GetAllPosts();
        void ToggleFavourite(int postId, int userId);
        List<Posts> GetFavouritePosts(int userId);
        List<Posts> GetAllPostsWithUsernames();

        Task CreatePostAsync(Posts post);
        Task UpdatePostAsync(Posts post);
        Task DeletePostAsync(int id);
        Task<Posts?> GetByIdAsync(int id);
        Task<List<Posts>> GetAllPostsAsync();
        Task ToggleFavouriteAsync(int postId, int userId);
        Task<List<Posts>> GetFavouritePostsAsync(int userId);
        Task<List<Posts>> GetAllPostsWithUsernamesAsync();

        
        List<ValidationResult> ValidateEntity<T>(T entity) where T : class;
        bool IsValid<T>(T entity) where T : class;
    }

    public class PostService : IPostService
    {
        private readonly Repository<Posts> _postRepository;
        private readonly Repository<Favourite> _favouriteRepository;
        private readonly Repository<User> _userRepository;

        public PostService(
            Repository<Posts> postRepository,
            Repository<Favourite> favouriteRepository,
            Repository<User> userRepository)
        {
            _postRepository = postRepository;
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
        }

 
        public void CreatePost(Posts post) => CreatePostAsync(post).Wait();
        public void UpdatePost(Posts post) => UpdatePostAsync(post).Wait();
        public void DeletePost(int id) => DeletePostAsync(id).Wait();
        public Posts? GetById(int id) => GetByIdAsync(id).Result;
        public List<Posts> GetAllPosts() => GetAllPostsAsync().Result;
        public void ToggleFavourite(int postId, int userId) => ToggleFavouriteAsync(postId, userId).Wait();
        public List<Posts> GetFavouritePosts(int userId) => GetFavouritePostsAsync(userId).Result;
        public List<Posts> GetAllPostsWithUsernames() => GetAllPostsWithUsernamesAsync().Result;

        public async Task CreatePostAsync(Posts post)
        {
            ValidatePost(post);

            var allPosts = await _postRepository.GetAllAsync();
            if (allPosts.Any(p => p.name == post.name))
            {
                throw new ValidationException("Post with this name already exists.");
            }

            await _postRepository.AddAsync(post);
        }

        public async Task UpdatePostAsync(Posts post)
        {
            ValidatePost(post);

            var existingPost = await _postRepository.GetByIdAsync(post.id);
            if (existingPost == null)
                throw new ValidationException("Post does not exist.");

            if (existingPost.UserId != post.UserId)
                throw new ValidationException("You don't have permission to edit this post.");

            await _postRepository.UpdateAsync(post);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new ValidationException("Post does not exist.");

            await _postRepository.DeleteAsync(id);
        }

        public async Task<Posts?> GetByIdAsync(int id)
            => await _postRepository.GetByIdAsync(id);

        public async Task<List<Posts>> GetAllPostsAsync()
            => await _postRepository.GetAllAsync();

        public async Task<List<Posts>> GetAllPostsWithUsernamesAsync()
        {
            var posts = await _postRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();

            return posts.Select(post =>
            {
                post.AuthorUsername = users.FirstOrDefault(u => u.id == post.UserId)?.Username ?? "deleted user";
                return post;
            }).ToList();
        }

        public async Task ToggleFavouriteAsync(int postId, int userId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
                throw new ValidationException("Post does not exist.");

            var allFavourites = await _favouriteRepository.GetAllAsync();
            var existingFavourite = allFavourites
                .FirstOrDefault(f => f.Postsid == postId && f.UserId == userId);

            if (existingFavourite != null)
            {
                await _favouriteRepository.DeleteAsync(existingFavourite.id);
                post.FavouriteCount = Math.Max(0, post.FavouriteCount - 1);
            }
            else
            {
                await _favouriteRepository.AddAsync(new Favourite
                {
                    Postsid = postId,
                    UserId = userId
                });
                post.FavouriteCount++;
            }

            await _postRepository.UpdateAsync(post);
        }

        public async Task<List<Posts>> GetFavouritePostsAsync(int userId)
        {
            var allFavourites = await _favouriteRepository.GetAllAsync();
            var favouritePosts = new List<Posts>();

            foreach (var fav in allFavourites.Where(f => f.UserId == userId))
            {
                var post = await _postRepository.GetByIdAsync(fav.Postsid);
                if (post != null) favouritePosts.Add(post);
            }

            return favouritePosts;
        }

        public List<ValidationResult> ValidateEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, true);
            return results;
        }

        public bool IsValid<T>(T entity) where T : class
            => ValidateEntity(entity).Count == 0;

        private void ValidatePost(Posts post)
        {
            var errors = ValidateEntity(post);
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors.Select(e => e.ErrorMessage)));
        }
    }
}