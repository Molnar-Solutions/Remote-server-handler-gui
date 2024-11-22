using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.utils
{
    public class APIResponse
    {
        public int StatusCode { get; set; } = 0;
        public string? Message { get; set; }
        public object? Data { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
    }
}
