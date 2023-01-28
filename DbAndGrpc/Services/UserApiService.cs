using DbAndGrpc.Model;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;


namespace DbAndGrpc.Services
{
    public class UserApiService : UserService.UserServiceBase
    {
        ApplicationContext db;
        public UserApiService(ApplicationContext db)
        {
            this.db = db;
        }
        // Отправка списка пользователей (На входе ничего,токо выход)
        public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
        {
            var listReply = new ListReply();
            var userList = db.Users.Select(x=> new UserReply { Id = x.Id, Name = x.Name,Age = x.Age }).ToList();
            listReply.Users.AddRange(userList);
            return Task.FromResult(listReply);
        }

        // Поиск по Id
        public override async Task<UserReply> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = await db.Users.FindAsync(request.Id);
            if (user == null)
                throw new RpcException(new Status(StatusCode.NotFound, "User не найден"));
            UserReply userReply = new UserReply() { Id=user.Id, Name = user.Name,Age = user.Age };
            return await Task.FromResult(userReply);
        }
        // Создание Юзера
        public override async Task<UserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Не введены данные"));
            var user = new User { Name = request.Name, Age = request.Age };
            if (await db.Users.FindAsync(user) != null)
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Юзер уже существует"));

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            var reply = new UserReply() { Id = user.Id, Name = user.Name,Age = user.Age };
            return await Task.FromResult(reply);
        }
    }
}
