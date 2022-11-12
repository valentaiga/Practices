// See https://aka.ms/new-console-template for more information

using Practices.gRPC.Client.Services;

var service = new BooksClient();
var book = await service.Get(1);
var createBook = await service.Create("titlllll", "d", 100);
var success = await service.Delete(createBook.Id);
var bookGet = await service.Get(createBook.Id);