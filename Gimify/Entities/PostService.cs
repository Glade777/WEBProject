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
        List<ValidationResult> ValidateEntity<T>(T entity) where T : class;
        bool IsValid<T>(T entity) where T : class;
    }

    public class PostService : IPostService
    {
        private readonly Repository<Posts> _postRepository;
        private readonly Repository<Favourite> _favouriteRepository;

        public PostService(Repository<Posts> postRepository, Repository<Favourite> favouriteRepository)
        {
            _postRepository = postRepository;
            _favouriteRepository = favouriteRepository;
        }

        public void CreatePost(Posts post)
        {
            
            ValidatePost(post);

            // унік. поста
            if (_postRepository.GetAll().Any(p => p.name == post.name))
            {
                throw new ValidationException("A post with this name already exists.");
            }

            _postRepository.Add(post);
        }

        public void UpdatePost(Posts post)
        {
            ValidatePost(post);

            var existingPost = _postRepository.GetById(post.id);
            if (existingPost == null)
            {
                throw new ValidationException("Post does not exist.");
            }

            if (existingPost.UserId != post.UserId)
            {
                throw new ValidationException("You do not have permission to edit this post.");
            }

            _postRepository.Update(post);
        }

        public void DeletePost(int id)
        {
            var post = _postRepository.GetById(id);
            if (post == null)
            {
                throw new ValidationException("Post does not exist.");
            }

            _postRepository.Delete(id);
        }

        public Posts? GetById(int id) => _postRepository.GetById(id);

        public List<Posts> GetAllPosts() => _postRepository.GetAll();

        public void ToggleFavourite(int postId, int userId)
        {
            var post = _postRepository.GetById(postId);
            if (post == null)
            {
                throw new ValidationException("Post does not exist.");
            }

            var existingFavourite = _favouriteRepository.GetAll()
                .FirstOrDefault(f => f.Postsid == postId && f.UserId == userId);

            if (existingFavourite != null)
            {
                _favouriteRepository.Delete(existingFavourite.id);
                post.FavouriteCount = Math.Max(0, post.FavouriteCount - 1);
            }
            else
            {
                var newFavourite = new Favourite
                {
                    Postsid = postId,
                    UserId = userId
                };
                _favouriteRepository.Add(newFavourite);
                post.FavouriteCount++;
            }

            _postRepository.Update(post);
        }

        public List<Posts> GetFavouritePosts(int userId)
        {
            return _favouriteRepository.GetAll()
                .Where(f => f.UserId == userId)
                .Select(f => _postRepository.GetById(f.Postsid))
                .Where(post => post != null)
                .ToList();
        }

        public List<ValidationResult> ValidateEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(entity, context, results, validateAllProperties: true);
            return results;
        }

        public bool IsValid<T>(T entity) where T : class => ValidateEntity(entity).Count == 0;

        private void ValidatePost(Posts post)
        {
            var validationResults = ValidateEntity(post);
            if (validationResults.Count > 0)
            {
                var messages = validationResults.Select(r => r.ErrorMessage);
                throw new ValidationException("Validation failed: " + string.Join("; ", messages));
            }
        }
    }
}