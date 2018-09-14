using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Antd.Native
{

    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    public sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeLibraryHandle() : base(true) 
        {}

        protected override bool ReleaseHandle() 
        {
            return UnsafeNativeMethods.FreeLibrary(this.handle);
        }
    }
}