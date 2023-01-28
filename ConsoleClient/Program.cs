using DbAndGrpc;
using Grpc.Core;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("http://localhost:5274");

var client = new UserService.UserServiceClient(channel);

ListReply users = await client.ListUsersAsync(new Google.Protobuf.WellKnownTypes.Empty());
Console.WriteLine("Все юзеры");
foreach (var user in users.Users)
{
    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
}
Console.WriteLine("Юзер с id 2");
UserReply userReply = await client.GetUserAsync(new GetUserRequest { Id = 2 });
Console.WriteLine($"{userReply.Id}.{userReply.Name} - {userReply.Age}");

// Добавление юзера
try
{
    Console.WriteLine("Добавляю юзера Ilya с возрастом 40");
    userReply = await client.CreateUserAsync(new CreateUserRequest { Name = "Ilya", Age = 40 });
    Console.WriteLine($"{userReply.Id}. {userReply.Name} - {userReply.Age}");
}
catch (RpcException ex)
{ Console.WriteLine(ex.Status.Detail ); }

Console.WriteLine("Обновление пользователя");
Console.WriteLine("Введите Id пользователя");
userReply = await client.UpdateUserAsync(new UpdateUserRequest { Id = Convert.ToInt32(Console.ReadLine()), Name = "IIIIIlya", Age = 20 });
Console.WriteLine($"{userReply.Id}. {userReply.Name} - {userReply.Age}");

Console.WriteLine("Удаление Юзера");
Console.WriteLine("Введите Id пользователя");
userReply = await client.DeleteUserAsync(new DeleteUserRequest { Id = Convert.ToInt32(Console.ReadLine()) });
Console.WriteLine($"Удален {userReply.Id}. {userReply.Name} - {userReply.Age}");