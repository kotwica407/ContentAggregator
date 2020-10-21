using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using ContentAggregator.Models.Model;
using ContentAggregator.Repositories;
using ContentAggregator.Repositories.Comments;
using ContentAggregator.Repositories.Hashes;
using ContentAggregator.Repositories.Pictures;
using ContentAggregator.Repositories.Posts;
using ContentAggregator.Repositories.Responses;
using ContentAggregator.Repositories.Tags;
using ContentAggregator.Repositories.Users;
using ContentAggregator.Services.Auth;
using ContentAggregator.Services.Comments;
using ContentAggregator.Services.Posts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ContentAggregator.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.ConfigureMapper();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHashRepository, HashRepository>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddHttpContextAccessor();
            services.ConfigureSwagger();

            services.ConfigureCors();
        }

        private static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("myAllowSpecificOrigins",
                    builder => { builder.WithOrigins("*"); });
            });
        }

        private static void ConfigureMapper(this IServiceCollection services)
        {
            var mappConfig = new MapperConfiguration(config =>
            {
                config.AddExpressionMapping();
                config.AddMapperProfiles();
            });
            services.AddScoped<IMapper>(_ => new Mapper(mappConfig));
        }

        private static void AddMapperProfiles(this IMapperConfigurationExpression configurationExpression)
        {
            Type profileType = typeof(Profile);
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
               .Where(x => x.FullName.StartsWith("ContentAggregator."))
               .SelectMany(a => a.GetTypes())
               .Where(p => profileType.IsAssignableFrom(p));
            foreach (Type type in types)
                configurationExpression.AddProfile(type);
        }

        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API CONTENT AGGREGATOR",
                        Version = "v1"
                    });

                options.IncludeComments();
            });
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CONTENT AGGREGATOR API V1");
            });
        }

        private static void IncludeComments(this SwaggerGenOptions options)
        {
            string[] documentationXmls = AppDomain.CurrentDomain.GetAssemblies()
               .Where(x => x.FullName.StartsWith("ContentAggregator"))
               .Select(x => Path.Combine(AppContext.BaseDirectory, $"{x.FullName.Split(',')[0]}.xml"))
               .Where(File.Exists)
               .ToArray();

            foreach (string documentationXml in documentationXmls)
                options.IncludeXmlComments(documentationXml);
        }
    }
}