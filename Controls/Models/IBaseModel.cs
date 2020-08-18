﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQC.Controls.Models
{
    public interface IBaseModel
    {

        bool IsNewTestModel { get; set; }

        bool IsDeleted { get; set; }

        void Save();

        void Disposed();
    }
}
