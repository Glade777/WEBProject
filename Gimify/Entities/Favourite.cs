﻿namespace Gimify.Entities
{
    public class Favourite
    {
        public int id { get; set; }
        public PostsMaker Posts { get; set; }
        public int Postsid { get; set; }
    }
}
