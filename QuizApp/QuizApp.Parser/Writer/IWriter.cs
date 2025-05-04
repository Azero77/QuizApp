using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDocumentTableParserProject.Writer
{
    /// <summary>
    /// Interface For Writing .docx file into  (stream,memory,db,....)
    /// </summary>
    public interface IWriter : IAsyncDisposable
    {
        Task WriteAsync(Question question);
    }
}
