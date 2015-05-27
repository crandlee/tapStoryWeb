using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace tapStoryWebApi.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<HttpPostedData> ParseMultipartAsync(this HttpContent postedContent)
        {
            var provider = await postedContent.ReadAsMultipartAsync();

            var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
            var fields = new Dictionary<string, HttpPostedField>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var content in provider.Contents)
            {
                var fieldName = content.Headers.ContentDisposition.Name.Trim('"');
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    var file = await content.ReadAsByteArrayAsync();
                    var fileName = content.Headers.ContentDisposition.FileName.Trim('"');
                    fileName = fileName.Substring(Path.GetPathRoot(fileName).Length);
                    files.Add(fieldName, new HttpPostedFile(fieldName, fileName, file));
                }
                else
                {
                    var data = await content.ReadAsStringAsync();
                    fields.Add(fieldName, new HttpPostedField(fieldName, data));
                }
            }

            return new HttpPostedData(fields, files);
        }
    }

    public class HttpPostedData
    {
        public HttpPostedData(IDictionary<string, HttpPostedField> fields, IDictionary<string, HttpPostedFile> files)
        {
            Fields = fields;
            Files = files;
        }

        public IDictionary<string, HttpPostedField> Fields { get; private set; }
        public IDictionary<string, HttpPostedFile> Files { get; private set; }
    }

    public class HttpPostedFile
    {
        private readonly string _name;
        private readonly string _fileName;
        private readonly IEnumerable<byte> _file;

        public HttpPostedFile(string name, string fileName, IEnumerable<byte> file, string serverId = null)
        {
            _name = name;
            _fileName = fileName;
            _file = file;
            ServerId = serverId;
        }

        public string Name { get { return _name; } }
        public string Filename { get { return _fileName; } }
        public IEnumerable<byte> File { get { return _file; } }

        public string ServerId { get; set; }
        public bool Exists { get; set; }
    }

    public class HttpPostedField
    {
        private readonly string _name;
        private readonly string _value;
        public HttpPostedField(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string Name { get { return _name; } }
        public string Value { get { return _value; } }
    }
}