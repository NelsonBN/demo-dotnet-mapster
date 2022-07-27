using System;

namespace APIApiDemo.DTOs;

public record PostRequest
{
    public string Author { get; set; }
    public string Text { get; set; }
    public bool Like { get; set; }
}

public record PostValueObject(string Author, string Text, bool Like);



public abstract record ArticleBase
{
    public DateTime OccurredAt { get; init; }

    protected ArticleBase()
        => OccurredAt = DateTime.UtcNow;
}

public record Article(string Author, string Text, bool Like) : ArticleBase;
