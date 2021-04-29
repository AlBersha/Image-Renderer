﻿using System.Collections.Generic;
using System.Numerics;
using ConverterBase.GeomHelper;
using Raytracer.ObjectProvider;

namespace Raytracer.Optimisation
{
    public interface ITreeProvider
    {
        public INode Root { get; set; }
        public ITreeProvider CreateTree(ObjectModel objectModel);
    }
}