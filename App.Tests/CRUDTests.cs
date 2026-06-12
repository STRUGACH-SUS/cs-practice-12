using Microsoft.EntityFrameworkCore;

namespace App.Tests;

public class CRUDTests
{
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public async void CreateUser_PassValid_Success(string name)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        await CRUD.Create(name);
        var result = db.Users.Select(x => x.Name).Contains(name);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void CreateUser_PassNull_Fail()
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Create(null).Wait());
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public async void CreateNote_PassValid_Success(string title)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User()
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        await CRUD.Create(title, user.Id);
        var result = db.Notes.Select(x => x.Title).Contains(title);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public async void CreateNote_PassError_Fail(string title)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User()
        {
            Name = "Sam"
        };
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Create(title, user.Id).Wait());
        await db.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void Create_PassNull_Fail()
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User()
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Create(null!,user.Id).Wait());
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData(1)]
    public async void ReadUser_PassValid_Success(int search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        db.Users.Add(new User()
        {
            Name = "Sam",
        });
        await db.SaveChangesAsync();
        var result = CRUD.Read(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    [InlineData(null)]
    public async void ReadUser_PassError_Fail(int search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var result = CRUD.Read(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.False(result);
        await db.Database.EnsureDeletedAsync();
    }

    [Theory]
    [InlineData("Text")]
    [InlineData("1")]
    [InlineData("")]
    public async void ReadNote_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var record = new Note
        {
            Title = search,
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        await db.SaveChangesAsync();
        var result = CRUD.Read(search).Result.Select(x => x.Title).Contains(search);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Text")]
    [InlineData("1")]
    [InlineData("")]
    [InlineData(null)]
    public async void ReadNote_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var result = CRUD.Read(search).Result.Select(x => x.Title).Contains(search);
        //Assert
        Assert.False(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData(1)]
    public async void ReadAllNotes_PassValid_Success(int search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        await db.SaveChangesAsync();
        var result = CRUD.ReadAllNotes(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    [InlineData(null)]
    public async void ReadAllNotes_PassError_Fail(int search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var result = CRUD.ReadAllNotes(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.False(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Mark")]
    [InlineData("6")]
    [InlineData("")]
    public async void UpdateUser_PassValid_Success(string changes)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam",
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        await CRUD.Update(user,changes);
        var result = db.Users.Select(x => x.Name).Contains(changes);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }

    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public async void UpdateUser_PassError_Fail(string changes)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam",
        };
        await CRUD.Update(user,changes);
        var result = user.Id == 0;
        //Assert
        Assert.False(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void UpdateUser_PassNull_Fail()
    {
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam",
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Update(user,null!).Wait());
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public async void UpdateNote_PassValid_Success(string changes)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        await db.SaveChangesAsync();
        await CRUD.Update(record,changes,user.Id);
        var result = db.Notes.Select(x => x.Title).Contains(changes);
        //Assert
        Assert.True(result);
        await db.Database.EnsureDeletedAsync();
    }

    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public async void UpdateNote_PassError_Fail(string changes)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = 0
        };
        //Assert
        Assert.Throws<AggregateException>(()=>CRUD.Update(record,changes,record.UserId).Wait());
        await db.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async void UpdateNote_PassNull_Fail()
    {
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        await db.SaveChangesAsync();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Update(record, null!, user.Id).Wait());
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public async void DeleteUser_PassValid_Success(string name)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = name
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        await CRUD.Delete(user);
        var result = db.Users.Select(x => x.Name).Contains(name);
        //Assert
        Assert.False(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public async void DeleteUser_PassError_Fail(string name)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = name
        };
        //Assert
        Assert.Throws<AggregateException>(()=>CRUD.Delete(user).Wait());
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public async void DeleteNote_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var record = new Note
        {
            Title = search,
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        await db.SaveChangesAsync();
        await CRUD.Delete(record);
        var result = db.Notes.Select(x => x.Title).Contains(search);
        //Assert
        Assert.False(result);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public async void DeleteNote_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = 0
        };
        //Assert
        Assert.Throws<AggregateException>(()=>CRUD.Delete(record).Wait());
    }
    
    [Theory]
    [InlineData(0,0)]
    [InlineData(1,1)]
    [InlineData(3,6)]
    [InlineData(5,0)]
    public async void GenerateData_PassValid_Success(int users, int records)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        await CRUD.GenerateData(users, records);
        var resultUsers = db.Users.Count();
        var resultRecords = db.Notes.Count();
        //Assert
        Assert.True(resultUsers == users && resultRecords == records*users);
        await db.Database.EnsureDeletedAsync();
    }
    
    [Theory]
    [InlineData(-1,-2)]
    [InlineData(1,-2)]
    [InlineData(-1,2)]
    public async void GenerateData_PassError_Fail(int users, int records)
    {
        //Act
        var db = new DataContext();
        await db.Database.EnsureCreatedAsync();
        await CRUD.GenerateData(users, records);
        var resultUsers = db.Users.Count();
        var resultRecords = db.Notes.Count();
        //Assert
        Assert.False(resultUsers == users && resultRecords == records*users);
        await db.Database.EnsureDeletedAsync();
    }
}