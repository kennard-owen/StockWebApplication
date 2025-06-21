using System;
using System.Collections.Generic;
using System.Text;

namespace PF.Core.Dto
{
    public class MenuDto
    {
        public string MenuId { get; set; }

        public string MenuName { get; set; }

        public List<MenuDto> Children { get; set; }
    }
}