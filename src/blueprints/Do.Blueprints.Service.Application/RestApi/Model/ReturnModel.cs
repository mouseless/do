﻿namespace Do.RestApi.Model;

public record ReturnModel(string Type,
    bool Async = false,
    bool Void = false
)
{
    public ReturnModel(bool async = false)
        : this(async ? typeof(Task).Name : "void", Async: async, Void: true) { }

    public string Type { get; set; } = Type;
    public bool Async { get; set; } = Async;
    public bool Void { get; set; } = Void;
}