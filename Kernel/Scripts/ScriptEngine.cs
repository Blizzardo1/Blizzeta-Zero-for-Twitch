/*‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾*\
|*  Copyright (C) 2014  Blizzeta Software                                   *|
|*                                                                          *|
|*  This program is free software: you can redistribute it and/or modify    *|
|*  it under the terms of the GNU General Public License as published by    *|
|*  the Free Software Foundation, either version 3 of the License, or       *|
|*  (at your option) any later version.                                     *|
|*                                                                          *|
|*  This program is distributed in the hope that it will be useful,         *|
|*  but WITHOUT ANY WARRANTY; without even the implied warranty of          *|
|*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *|
|*  GNU General Public License for more details.                            *|
|*                                                                          *|
|*  You should have received a copy of the GNU General Public License       *|
|*  along with this program.  If not, see <http://www.gnu.org/licenses/>.   *|
\*__________________________________________________________________________*/

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlizzetaZero.Kernel.Scripts
{
    public enum Permissions : int
    {
        Root = 0,
        Administrator = 1,
        Streamer = 2,
        Operator = 3,
        User = 4,
        Guest = 5
    }

    public class ScriptContext
    {
        public ScriptContext ( )
        {
            Parameters = new Dictionary<object, object> ( );
        }

        public object Arguments { get; set; }

        public Channel channel { get; set; }

        public Exception Error { get; set; }

        public string nick { get; set; }

        public Dictionary<object, object> Parameters { get; set; }

        public object Result { get; set; }

        public Irc server { get; set; }
    }

    public class ScriptEngine
    {
        private readonly string[] DEFAULT_ASSEMBLIES;
        private readonly string[] DEFAULT_NAMESPACES;

        public ScriptEngine ( )
        {
            DEFAULT_ASSEMBLIES = new string[] { "System.dll", "System.Core.dll", "System.Data.dll", "System.Xml.dll", "System.Xml.Linq.dll", System.Reflection.Assembly.GetExecutingAssembly ( ).GetName ( ).Name + ".exe" };
            DEFAULT_NAMESPACES = new string[] { "System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Xml", "BlizzetaZero", "BlizzetaZero.Kernel", "BlizzetaZero.Kernel.Scripts" };

            ReferencedAssemblies = new List<string> ( );
            UsingNamespaces = new List<string> ( );
        }

        public ScriptEngine ( string[] assemblies, string[] namespaces )
        {
            DEFAULT_ASSEMBLIES = assemblies;
            DEFAULT_NAMESPACES = namespaces;
            ReferencedAssemblies = new List<string> ( );
            UsingNamespaces = new List<string> ( );
        }

        public List<string> ReferencedAssemblies { get; set; }

        public List<string> UsingNamespaces { get; set; }

        public ScriptObjects GetInterface ( string filename )
        {
            if ( !filename.EndsWith ( ".cs" ) )
            {
                // Filename != C# Source
                return null;
            }
            string sourceCode = File.ReadAllText ( filename );
            bool compilationSucceeded = true;
            CSharpCodeProvider provider = new CSharpCodeProvider ( );

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters ( );
            cp.TreatWarningsAsErrors = false;

            // Add assembly references.
            cp.ReferencedAssemblies.AddRange ( DEFAULT_ASSEMBLIES );
            cp.ReferencedAssemblies.AddRange ( ReferencedAssemblies.ToArray ( ) );

            cp.GenerateInMemory = true;

            // Add using statements.
            StringBuilder script = new StringBuilder ( );
            foreach ( var usingNamespace in DEFAULT_NAMESPACES )
                script.AppendFormat ( "using {0};\n", usingNamespace );

            foreach ( var additionalUsingNamespace in UsingNamespaces )
                script.AppendFormat ( "using {0};\n", additionalUsingNamespace );

            // Create the script.
            script.AppendLine ( "namespace BlizzetaZero.Kernel.Scripts" );
            script.AppendLine ( "{" );
            script.AppendLine ( sourceCode );
            script.AppendLine ( "}" );

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromSource ( cp, script.ToString ( ) );

            if ( cr.Errors.Count > 0 )
            {
                foreach ( var error in cr.Errors )
                {
                    Console.WriteLine ( error );
                }
                compilationSucceeded = false;
            }

            if ( compilationSucceeded )
            {
                var ass = cr.CompiledAssembly;
                var execInstance = ass.CreateInstance ( "BlizzetaZero.Kernel.Scripts.Script" );
                var po = GetProperties ( execInstance );

                return new ScriptObjects (
                    ( string ) po[ "get_Name" ],
                    ( string ) po[ "get_Usage" ],
                    ( string ) po[ "get_Help" ],
                    ( string ) po[ "get_More" ],
                    ( string ) po[ "get_Version" ],
                    ( Permissions ) po[ "get_Permission" ],
                    ( bool ) po[ "get_IsPublic" ] );
            }

            return null;
        }

        public bool Run ( string filename, ref ScriptContext context )
        {
            CompilerErrorCollection compilatorErrors = new CompilerErrorCollection ( );
            return this.Run ( filename, ref context, out compilatorErrors );
        }

        public CompilerErrorCollection RunSource ( string source, ref ScriptContext context )
        {
            bool compilationSucceeded = true;
            CompilerErrorCollection compilationErrors = new CompilerErrorCollection ( );
            CSharpCodeProvider provider = new CSharpCodeProvider ( );

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters ( );
            cp.TreatWarningsAsErrors = false;

            // Add assembly references.
            cp.ReferencedAssemblies.AddRange ( DEFAULT_ASSEMBLIES );
            cp.ReferencedAssemblies.AddRange ( ReferencedAssemblies.ToArray ( ) );

            cp.GenerateInMemory = true;

            // Add using statements.
            StringBuilder script = new StringBuilder ( );
            foreach ( var usingNamespace in DEFAULT_NAMESPACES )
                script.AppendFormat ( "using {0};\r\n", usingNamespace );

            foreach ( var additionalUsingNamespace in UsingNamespaces )
                script.AppendFormat ( "using {0};\r\n", additionalUsingNamespace );

            // Create the script.
            script.AppendLine ( );
            script.AppendLine ( "namespace BlizzetaZero.Kernel.Scripts" );
            script.AppendLine ( "{" );
            script.AppendFormat ( "\t{0}", source );
            script.AppendLine ( "}" );

            Console.WriteLine ( script.ToString ( ) );

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromSource ( cp, script.ToString ( ) );

            if ( cr.Errors.Count > 0 )
            {
                foreach ( CompilerError ce in cr.Errors )
                {
#if DEBUG
                    Console.WriteLine ( "->\t{0}", ce.ToString ( ) );
                    Console.WriteLine ( );
#endif
                    ce.Line = ce.Line - 13;
                    compilationErrors.Add ( ce );

                    if ( !ce.IsWarning )
                        compilationSucceeded = false;
                }
            }

            if ( compilationSucceeded )
            {
                var ass = cr.CompiledAssembly;
                var execInstance = ass.CreateInstance ( "BlizzetaZero.Kernel.Scripts.Script" );

                var type = execInstance.GetType ( );
                var methodInfo = type.GetMethod ( "Load" );

                // Execute the code.
                methodInfo.Invoke ( execInstance, new object[] { context } );
            }

            return compilationErrors;
        }

        public bool Run ( string filename, ref ScriptContext context, out CompilerErrorCollection compilationErrors )
        {
            string sourceCode = File.ReadAllText ( filename );
            bool compilationSucceeded = true;
            compilationErrors = new CompilerErrorCollection ( );
            CSharpCodeProvider provider = new CSharpCodeProvider ( );

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters ( );
            cp.TreatWarningsAsErrors = false;

            // Add assembly references.
            cp.ReferencedAssemblies.AddRange ( DEFAULT_ASSEMBLIES );
            cp.ReferencedAssemblies.AddRange ( ReferencedAssemblies.ToArray ( ) );

            cp.GenerateInMemory = true;

            // Add using statements.
            StringBuilder script = new StringBuilder ( );
            foreach ( var usingNamespace in DEFAULT_NAMESPACES )
                script.AppendFormat ( "using {0};\n", usingNamespace );

            foreach ( var additionalUsingNamespace in UsingNamespaces )
                script.AppendFormat ( "using {0};\n", additionalUsingNamespace );

            // Create the script.
            script.AppendLine ( );
            script.AppendLine ( "namespace BlizzetaZero.Kernel.Scripts" );
            script.AppendLine ( "{" );
            script.AppendLine ( sourceCode );
            script.AppendLine ( "}" );

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromSource ( cp, script.ToString ( ) );

            if ( cr.Errors.Count > 0 )
            {
                foreach ( CompilerError ce in cr.Errors )
                {
#if DEBUG
                    Console.WriteLine ( "  {0}", ce.ToString ( ) );
                    Console.WriteLine ( );
#endif
                    ce.Line = ce.Line - 13;
                    compilationErrors.Add ( ce );

                    if ( !ce.IsWarning )
                        compilationSucceeded = false;
                }
            }

            if ( compilationSucceeded )
            {
                var ass = cr.CompiledAssembly;
                var execInstance = ass.CreateInstance ( "BlizzetaZero.Kernel.Scripts.Script" );

                var type = execInstance.GetType ( );
                var methodInfo = type.GetMethod ( "Load" );

                // Execute the code.
                methodInfo.Invoke ( execInstance, new object[] { context } );
            }

            return compilationSucceeded;
        }

        private static Dictionary<string, object> GetProperties ( object obj )
        {
            Dictionary<string, object> lost = new Dictionary<string, object> ( );
            foreach ( var pinfo in obj.GetType ( ).GetProperties ( ) )
            {
                var getMethod = pinfo.GetGetMethod ( );
                if ( getMethod.ReturnType.IsPublic )
                {
                    lost.Add ( getMethod.Name, getMethod.Invoke ( obj, null ) );
                }
            }
            return lost;
        }
    }

    public class ScriptObjects
    {
        private string _help, _more, _name, _usage, _version;
        private bool _isPublic;
        private Permissions _permission;

        public ScriptObjects ( string Name, string Usage, string Help, string More, string Version, Permissions Permission, bool isPublic )
        {
            _name = Name;
            _usage = Usage;
            _help = Help;
            _more = More;
            _version = Version;
            _permission = Permission;
            _isPublic = isPublic;
        }

        public string Help
        {
            get { return _help; }
        }

        public bool IsPublic
        {
            get { return _isPublic; }
        }

        public string More
        {
            get { return _more; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Permissions Permission
        {
            get { return _permission; }
        }

        public string Usage
        {
            get { return _usage; }
        }

        public string Version
        {
            get { return _version; }
        }
    }
}