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
        // Отправка списка пользователей
        public override Task<ListReply> ListUsers(Empty request, ServerCallContext context)
        {
            var listReply = new ListReply();
            var userList = db.Users.Select(x=> new UserReply { Id = x.Id, Name = x.Name,Age = x.Age }).ToList();
            listReply.Users.AddRange(userList);
            return Task.FromResult(listReply);
        }
    }
}
