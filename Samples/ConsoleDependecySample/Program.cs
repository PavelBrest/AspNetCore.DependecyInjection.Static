using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.DependecyInjection.Static.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleDependecySample
{
    class Program
    {
        //public static string A = "A";
        
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScopedStatic<TodoService>();
            serviceCollection.AddScopedStatic<ITodoRepository, TodoRepository>();

            AspNetCore.DependecyInjection.Static.StaticModule.Register(serviceCollection);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var serviceScope = serviceProvider.CreateScope();
            var todoService = serviceScope.ServiceProvider.GetService<TodoService>();

            await todoService.CreateAsync(Guid.NewGuid().ToString());
            await todoService.CreateAsync(Guid.NewGuid().ToString());
            await todoService.CreateAsync(Guid.NewGuid().ToString());
            await todoService.CreateAsync(Guid.NewGuid().ToString());
            await todoService.CreateAsync(Guid.NewGuid().ToString());

            foreach (var todo in await todoService.GetList())
            {
                Console.WriteLine($"{{ Title: {todo.Title}; CreatedAt: {todo.CreatedAt}; }}");
            }
                
            serviceScope.Dispose();
            
            Console.ReadKey();
        }
    }

    public class TodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task CreateAsync(string title)
        {
            await _repository.CreateAsync(new Todo
            {
                Title = title,
                CreatedAt = DateTime.UtcNow
            });
        }

        public Task<ICollection<Todo>> GetList() => _repository.GetListAsync();
    }

    public class TodoRepository : ITodoRepository
    {
        private readonly List<Todo> _list = new();
        
        public Task<ICollection<Todo>> GetListAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult((ICollection<Todo>)_list);
        }

        public Task CreateAsync(Todo entity, CancellationToken cancellationToken)
        {
            _list.Add(entity);
            
            return Task.CompletedTask;
        }
    }
    
    public interface ITodoRepository
    {
        Task<ICollection<Todo>> GetListAsync(CancellationToken cancellationToken = default);
        Task CreateAsync(Todo entity, CancellationToken cancellationToken = default);
    }

    public class Todo
    {
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
    }
}