using Domain.Exceptions;

namespace Domain.Errors;

public static class Errors
{
    public static readonly BusinessException FileNotFound = new("File not found", "Storage");
    public static readonly BusinessException FolderNotFound = new("Folder not found", "Storage");
    public static readonly BusinessException PleaseSelectAFile = new("Please select a file", "Storage");
}