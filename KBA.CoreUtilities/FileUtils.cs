using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBA.CoreUtilities.Utilities
{
    /// <summary>
    /// Provides file manipulation utilities
    /// </summary>
    public static class FileUtils
    {
        #region File Operations

        /// <summary>
        /// Reads all text from a file asynchronously
        /// </summary>
        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return await File.ReadAllTextAsync(filePath);
        }

        /// <summary>
        /// Writes text to a file asynchronously
        /// </summary>
        public static async Task WriteAllTextAsync(string filePath, string content)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await File.WriteAllTextAsync(filePath, content);
        }

        /// <summary>
        /// Appends text to a file asynchronously
        /// </summary>
        public static async Task AppendTextAsync(string filePath, string content)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await File.AppendAllTextAsync(filePath, content);
        }

        /// <summary>
        /// Reads all lines from a file
        /// </summary>
        public static string[] ReadAllLines(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return File.ReadAllLines(filePath);
        }

        /// <summary>
        /// Reads file in chunks (useful for large files)
        /// </summary>
        public static IEnumerable<string> ReadLinesLazy(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return File.ReadLines(filePath);
        }

        #endregion

        #region File Information

        /// <summary>
        /// Gets file size in bytes
        /// </summary>
        public static long GetFileSize(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return new FileInfo(filePath).Length;
        }

        /// <summary>
        /// Gets human-readable file size (KB, MB, GB, etc.)
        /// </summary>
        public static string GetFileSizeFormatted(string filePath)
        {
            var bytes = GetFileSize(filePath);
            return FormatFileSize(bytes);
        }

        /// <summary>
        /// Formats bytes to human-readable size
        /// </summary>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            var order = 0;
            
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Gets file extension
        /// </summary>
        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath).TrimStart('.');
        }

        /// <summary>
        /// Gets file name without extension
        /// </summary>
        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        /// Gets file creation time
        /// </summary>
        public static DateTime GetFileCreationTime(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return File.GetCreationTime(filePath);
        }

        /// <summary>
        /// Gets file last modification time
        /// </summary>
        public static DateTime GetFileLastModifiedTime(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", filePath);

            return File.GetLastWriteTime(filePath);
        }

        #endregion

        #region MIME Type Detection

        private static readonly Dictionary<string, string> MimeTypes = new()
        {
            // Images
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".webp", "image/webp" },
            { ".svg", "image/svg+xml" },
            { ".ico", "image/x-icon" },
            { ".tiff", "image/tiff" },
            
            // Documents
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".ppt", "application/vnd.ms-powerpoint" },
            { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { ".txt", "text/plain" },
            { ".csv", "text/csv" },
            { ".rtf", "application/rtf" },
            
            // Archives
            { ".zip", "application/zip" },
            { ".rar", "application/x-rar-compressed" },
            { ".7z", "application/x-7z-compressed" },
            { ".tar", "application/x-tar" },
            { ".gz", "application/gzip" },
            
            // Audio
            { ".mp3", "audio/mpeg" },
            { ".wav", "audio/wav" },
            { ".ogg", "audio/ogg" },
            { ".m4a", "audio/mp4" },
            
            // Video
            { ".mp4", "video/mp4" },
            { ".avi", "video/x-msvideo" },
            { ".mov", "video/quicktime" },
            { ".wmv", "video/x-ms-wmv" },
            { ".flv", "video/x-flv" },
            { ".webm", "video/webm" },
            
            // Web
            { ".html", "text/html" },
            { ".htm", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/javascript" },
            { ".json", "application/json" },
            { ".xml", "application/xml" },
            
            // Programming
            { ".cs", "text/plain" },
            { ".java", "text/plain" },
            { ".py", "text/plain" },
            { ".cpp", "text/plain" },
            { ".c", "text/plain" },
            { ".h", "text/plain" },
        };

        /// <summary>
        /// Gets MIME type for a file
        /// </summary>
        public static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return MimeTypes.TryGetValue(extension, out var mimeType) 
                ? mimeType 
                : "application/octet-stream";
        }

        /// <summary>
        /// Checks if file is an image
        /// </summary>
        public static bool IsImageFile(string filePath)
        {
            var mimeType = GetMimeType(filePath);
            return mimeType.StartsWith("image/");
        }

        /// <summary>
        /// Checks if file is a document
        /// </summary>
        public static bool IsDocumentFile(string filePath)
        {
            var extension = GetFileExtension(filePath).ToLowerInvariant();
            return extension is "pdf" or "doc" or "docx" or "xls" or "xlsx" or "ppt" or "pptx" or "txt" or "csv" or "rtf";
        }

        /// <summary>
        /// Checks if file is a video
        /// </summary>
        public static bool IsVideoFile(string filePath)
        {
            var mimeType = GetMimeType(filePath);
            return mimeType.StartsWith("video/");
        }

        /// <summary>
        /// Checks if file is audio
        /// </summary>
        public static bool IsAudioFile(string filePath)
        {
            var mimeType = GetMimeType(filePath);
            return mimeType.StartsWith("audio/");
        }

        #endregion

        #region Compression

        /// <summary>
        /// Compresses a file using GZip
        /// </summary>
        public static async Task CompressFileAsync(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Source file not found", sourceFile);

            await using var sourceStream = File.OpenRead(sourceFile);
            await using var destinationStream = File.Create(destinationFile);
            await using var compressionStream = new GZipStream(destinationStream, CompressionMode.Compress);
            
            await sourceStream.CopyToAsync(compressionStream);
        }

        /// <summary>
        /// Decompresses a GZip compressed file
        /// </summary>
        public static async Task DecompressFileAsync(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Source file not found", sourceFile);

            await using var sourceStream = File.OpenRead(sourceFile);
            await using var destinationStream = File.Create(destinationFile);
            await using var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress);
            
            await decompressionStream.CopyToAsync(destinationStream);
        }

        /// <summary>
        /// Creates a ZIP archive from multiple files
        /// </summary>
        public static void CreateZipArchive(string[] sourceFiles, string zipFilePath)
        {
            if (sourceFiles == null || sourceFiles.Length == 0)
                throw new ArgumentException("Source files cannot be empty");

            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);

            using var archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
            
            foreach (var file in sourceFiles)
            {
                if (File.Exists(file))
                {
                    var fileName = Path.GetFileName(file);
                    archive.CreateEntryFromFile(file, fileName);
                }
            }
        }

        /// <summary>
        /// Extracts a ZIP archive
        /// </summary>
        public static void ExtractZipArchive(string zipFilePath, string destinationDirectory)
        {
            if (!File.Exists(zipFilePath))
                throw new FileNotFoundException("ZIP file not found", zipFilePath);

            if (!Directory.Exists(destinationDirectory))
                Directory.CreateDirectory(destinationDirectory);

            ZipFile.ExtractToDirectory(zipFilePath, destinationDirectory, true);
        }

        /// <summary>
        /// Compresses a directory to ZIP
        /// </summary>
        public static void CompressDirectory(string sourceDirectory, string zipFilePath)
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException($"Directory not found: {sourceDirectory}");

            if (File.Exists(zipFilePath))
                File.Delete(zipFilePath);

            ZipFile.CreateFromDirectory(sourceDirectory, zipFilePath);
        }

        #endregion

        #region File Comparison

        /// <summary>
        /// Compares two files byte by byte
        /// </summary>
        public static bool AreFilesEqual(string file1, string file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2))
                return false;

            var info1 = new FileInfo(file1);
            var info2 = new FileInfo(file2);

            // Quick check: if sizes differ, files are different
            if (info1.Length != info2.Length)
                return false;

            // Compare byte by byte
            using var fs1 = File.OpenRead(file1);
            using var fs2 = File.OpenRead(file2);

            var bufferSize = 4096;
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var read1 = fs1.Read(buffer1, 0, bufferSize);
                var read2 = fs2.Read(buffer2, 0, bufferSize);

                if (read1 != read2)
                    return false;

                if (read1 == 0)
                    break;

                for (var i = 0; i < read1; i++)
                {
                    if (buffer1[i] != buffer2[i])
                        return false;
                }
            }

            return true;
        }

        #endregion

        #region Directory Operations

        /// <summary>
        /// Gets all files in directory recursively
        /// </summary>
        public static string[] GetAllFiles(string directory, string searchPattern = "*.*")
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"Directory not found: {directory}");

            return Directory.GetFiles(directory, searchPattern, SearchOption.AllDirectories);
        }

        /// <summary>
        /// Gets total size of all files in directory
        /// </summary>
        public static long GetDirectorySize(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"Directory not found: {directory}");

            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            return files.Sum(file => new FileInfo(file).Length);
        }

        /// <summary>
        /// Copies directory recursively
        /// </summary>
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true)
        {
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            var dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (var file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            if (recursive)
            {
                foreach (var subDir in dirs)
                {
                    var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        /// <summary>
        /// Deletes directory and all contents
        /// </summary>
        public static void DeleteDirectory(string directory, bool recursive = true)
        {
            if (!Directory.Exists(directory))
                return;

            Directory.Delete(directory, recursive);
        }

        /// <summary>
        /// Ensures directory exists (creates if not)
        /// </summary>
        public static void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        #endregion

        #region Path Operations

        /// <summary>
        /// Combines multiple path segments
        /// </summary>
        public static string CombinePaths(params string[] paths)
        {
            if (paths == null || paths.Length == 0)
                throw new ArgumentException("Paths cannot be empty");

            return Path.Combine(paths);
        }

        /// <summary>
        /// Gets relative path from base directory
        /// </summary>
        public static string GetRelativePath(string fromPath, string toPath)
        {
            var fromUri = new Uri(AppendDirectorySeparator(fromPath));
            var toUri = new Uri(toPath);

            var relativeUri = fromUri.MakeRelativeUri(toUri);
            var relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            return relativePath.Replace('/', Path.DirectorySeparatorChar);
        }

        private static string AppendDirectorySeparator(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path + Path.DirectorySeparatorChar;
            
            return path;
        }

        /// <summary>
        /// Sanitizes file name (removes invalid characters)
        /// </summary>
        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return fileName;

            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new StringBuilder(fileName);

            foreach (var c in invalidChars)
            {
                sanitized.Replace(c, '_');
            }

            return sanitized.ToString();
        }

        #endregion

        #region Temporary Files

        /// <summary>
        /// Creates a temporary file and returns its path
        /// </summary>
        public static string CreateTempFile(string extension = ".tmp")
        {
            var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
            File.Create(tempPath).Dispose();
            return tempPath;
        }

        /// <summary>
        /// Creates a temporary directory and returns its path
        /// </summary>
        public static string CreateTempDirectory()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        #endregion
    }
}
