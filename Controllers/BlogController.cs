﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CacheExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private IMemoryCache cache;
        private ExampleDbContext dbContext;
        public BlogController(IMemoryCache cache, ExampleDbContext dbContext)
        {
            this.cache = cache;
            this.dbContext = dbContext;
        }
     
     
        [HttpGet]
        public IEnumerable<Blog> Get()
        {
            if (cache.TryGetValue("blogs", out IEnumerable<Blog> blogs))
            {
                return blogs;
            }

            blogs = dbContext.Blogs.Where(m => m.IsActive == true).ToList();

            MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
            memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            cache.Set("blogs", blogs, memoryCacheEntryOptions);

            return blogs;
        }

        
    }
}
