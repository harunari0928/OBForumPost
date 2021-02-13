using OBForumPost.Domain.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBFormPost.Application.ViewModel
{
    public sealed class PostFountViewModel
    {
        public long PostId { get; set; }
        public string Status { get; private set; }
        public DateTime PostDateTime { get; set; }
        public string Title { get; private set; }
    }
}
