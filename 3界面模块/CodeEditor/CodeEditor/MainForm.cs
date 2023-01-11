using CodeEditor.CodeEdit;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            //【1】新建C#代码生成器和代码编译器的实例
            CSharpCodeProvider Provider = new CSharpCodeProvider();
            //【2】使用指定的编译器，从包含源代码的字符串设置编译程序集
            ICodeCompiler objICodeCompiler = Provider.CreateCompiler();
            //【3】配置用于调用编译器的参数
            CompilerParameters Parameters = new CompilerParameters();
            Parameters.ReferencedAssemblies.Add("System.dll");
            Parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            Parameters.ReferencedAssemblies.Add("System.Linq.dll");
            Parameters.GenerateExecutable = false;
            Parameters.GenerateInMemory = true;
            //【4】启动编译
            CompilerResults Result = objICodeCompiler.CompileAssemblyFromSource(Parameters, rtbCode.Text);
            if (Result.Errors.HasErrors)
            {
                Console.WriteLine("编译错误：");
                foreach (CompilerError err in Result.Errors)
                {
                    AppendInfo(err.ErrorText);
                }
            }
            else
            {
                // 通过反射，调用实例 
                Assembly objAssembly = Result.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("CodeEditor.CodeEdit.Code");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("Test");
                object cc = objMI.Invoke(objHelloWorld, null);
                AppendInfo(cc);
            }
        }
        //追加字符
        private void AppendInfo(object Info)
        {
            rtbResult.Text =Info+"\n\r";
        }
    }
}
