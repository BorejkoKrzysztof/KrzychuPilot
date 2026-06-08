using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrzychuPilot.Application.Common.Models.CreatePromptGroupDto
{
    public class CreatePromptGroupDto
    {
        public List<string> Prompts { get; set; } = new();
    }

}
