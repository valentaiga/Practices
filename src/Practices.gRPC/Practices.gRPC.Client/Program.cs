﻿// See https://aka.ms/new-console-template for more information

using Practices.gRPC.Client.Services;

var service = new BooksClient();
await service.Get(1);
var createBook = await service.Create("titlllll", "d", 100);
await service.Delete(createBook.Id);
await service.Get(createBook.Id);