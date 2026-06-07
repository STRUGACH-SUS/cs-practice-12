using Microsoft.EntityFrameworkCore;

namespace App.Tests;

public class CRUDTests
{
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public void CreateUser_PassValid_Success(string name)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.Create(name).Wait();
        var result = db.Users.Select(x => x.Name).Contains(name);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }
    
    [Fact]
    public void CreateUser_PassNull_Fail()
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Create(null).Wait());
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public void CreateNote_PassValid_Success(string title)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User()
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        CRUD.Create(title, user.Id).Wait();
        var result = db.Notes.Select(x => x.Title).Contains(title);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Name")]
    [InlineData("1")]
    [InlineData("")]
    public void CreateNote_PassError_Fail(string title)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User()
        {
            Name = "Sam"
        };
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Create(title, user.Id).Wait());
        db.Database.EnsureDeleted();
    }
    
    [Fact]
    public void Create_PassNull_Fail()
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User()
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Create(null!,user.Id).Wait());
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData(1)]
    public void ReadUser_PassValid_Success(int search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        db.Users.Add(new User()
        {
            Name = "Sam",
        });
        db.SaveChangesAsync().Wait();
        var result = CRUD.Read(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    [InlineData(null)]
    public void ReadUser_PassError_Fail(int search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var result = CRUD.Read(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.False(result);
        db.Database.EnsureDeleted();
    }

    [Theory]
    [InlineData("Text")]
    [InlineData("1")]
    [InlineData("")]
    public void ReadNote_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        var record = new Note
        {
            Title = search,
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        db.SaveChanges();
        var result = CRUD.Read(search).Result.Select(x => x.Title).Contains(search);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Text")]
    [InlineData("1")]
    [InlineData("")]
    [InlineData(null)]
    public void ReadNote_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var result = CRUD.Read(search).Result.Select(x => x.Title).Contains(search);
        //Assert
        Assert.False(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData(1)]
    public void ReadAllNotes_PassValid_Success(int search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        db.SaveChanges();
        var result = CRUD.ReadAllNotes(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    [InlineData(null)]
    public void ReadAllNotes_PassError_Fail(int search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var result = CRUD.ReadAllNotes(search).Result.Select(x => x.Id).Contains(search);
        //Assert
        Assert.False(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Mark")]
    [InlineData("6")]
    [InlineData("")]
    public void UpdateUser_PassValid_Success(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam",
        };
        db.Users.Add(user);
        db.SaveChanges();
        CRUD.Update(user,changes).Wait();
        var result = db.Users.Select(x => x.Name).Contains(changes);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }

    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public void UpdateUser_PassError_Fail(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam",
        };
        CRUD.Update(user,changes).Wait();
        var result = user.Id == 0;
        //Assert
        Assert.False(result);
        db.Database.EnsureDeleted();
    }
    
    [Fact]
    public void UpdateUser_PassNull_Fail()
    {
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam",
        };
        db.Users.Add(user);
        db.SaveChanges();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Update(user,null!).Wait());
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public void UpdateNote_PassValid_Success(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        db.SaveChanges();
        CRUD.Update(record,changes,user.Id).Wait();
        var result = db.Notes.Select(x => x.Title).Contains(changes);
        //Assert
        Assert.True(result);
        db.Database.EnsureDeleted();
    }

    [Theory]
    [InlineData("Rank")]
    [InlineData("6")]
    [InlineData("")]
    public void UpdateNote_PassError_Fail(string changes)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = 0
        };
        //Assert
        Assert.Throws<AggregateException>(()=>CRUD.Update(record,changes,record.UserId).Wait());
        db.Database.EnsureDeleted();
    }
    
    [Fact]
    public void UpdateNote_PassNull_Fail()
    {
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        var record = new Note
        {
            Title = "search",
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        db.SaveChanges();
        //Assert
        Assert.Throws<AggregateException>(() => CRUD.Update(record, null!, user.Id).Wait());
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public void DeleteUser_PassValid_Success(string name)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = name
        };
        db.Users.Add(user);
        db.SaveChanges();
        CRUD.Delete(user).Wait();
        var result = db.Users.Select(x => x.Name).Contains(name);
        //Assert
        Assert.False(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public void DeleteUser_PassError_Fail(string name)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
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
    public void DeleteNote_PassValid_Success(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        var user = new User
        {
            Name = "Sam"
        };
        db.Users.Add(user);
        db.SaveChanges();
        var record = new Note
        {
            Title = search,
            CreatedAt = DateTimeOffset.Now,
            UserId = user.Id,
        };
        db.Notes.Add(record);
        db.SaveChanges();
        CRUD.Delete(record).Wait();
        var result = db.Notes.Select(x => x.Title).Contains(search);
        //Assert
        Assert.False(result);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData("Level")]
    [InlineData("7")]
    [InlineData("")]
    public void DeleteNote_PassError_Fail(string search)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
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
    public void GenerateData_PassValid_Success(int users, int records)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.GenerateData(users, records).Wait();
        var resultUsers = db.Users.Count();
        var resultRecords = db.Notes.Count();
        //Assert
        Assert.True(resultUsers == users && resultRecords == records*users);
        db.Database.EnsureDeleted();
    }
    
    [Theory]
    [InlineData(-1,-2)]
    [InlineData(1,-2)]
    [InlineData(-1,2)]
    public void GenerateData_PassError_Fail(int users, int records)
    {
        //Act
        var db = new DataContext();
        db.Database.EnsureCreated();
        CRUD.GenerateData(users, records).Wait();
        var resultUsers = db.Users.Count();
        var resultRecords = db.Notes.Count();
        //Assert
        Assert.False(resultUsers == users && resultRecords == records*users);
        db.Database.EnsureDeleted();
    }
}