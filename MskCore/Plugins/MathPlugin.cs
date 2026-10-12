using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace MskCore.Plugins;

public class MathPlugin
{
    [KernelFunction("math_add")]
    [Description("Add two decimal values together")]
    public async Task<float> Addition(float a, float b)
    {
        return a + b;
    }

    [KernelFunction("math_sub")]
    [Description("Subtract two decimal values together")]
    public async Task<float> Subtraction(float a, float b)
    {
        return a - b;
    }

    [KernelFunction("math_list_add")]
    [Description("Adds each decimal value of list a to the decimal value at the same index in list b. Size of list a must be same as size b, output will be the same size.")]
    public async Task<List<float>> ListAddition(List<float> a, List<float> b)
    {
        return a.Count != b.Count ? throw new IndexOutOfRangeException() : a.Zip(b, (x, y) => x + y).ToList();
    }
    
    [KernelFunction("math_list_subtract")]
    [Description("Subtracts each decimal value of list b from the decimal value at the same index in list a. Size of list a must be same as size b, output will be the same size.")]
    public async Task<List<float>> ListSubtraction(List<float> a, List<float> b)
    {
        return a.Count != b.Count ? throw new IndexOutOfRangeException() : a.Zip(b, (x, y) => x - y).ToList();
    }
}