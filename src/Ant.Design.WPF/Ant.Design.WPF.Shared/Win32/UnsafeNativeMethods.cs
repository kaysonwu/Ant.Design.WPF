namespace Antd.Win32
{
    using Standard;
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Text;

    [SuppressUnmanagedCodeSecurity]
    public static class UnsafeNativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow([In] IntPtr handle, [In] MonitorOptions flags);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms647486%28v=vs.85%29.aspx</devdoc>
        [DllImport("user32", CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "LoadStringW", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern int LoadString([In] [Optional] SafeLibraryHandle hInstance, [In] uint uID, [Out] StringBuilder lpBuffer, [In] int nBufferMax);

        ///<devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms684175%28v=vs.85%29.aspx</devdoc>
        [DllImport("kernel32", CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "LoadLibraryW", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        public static extern SafeLibraryHandle LoadLibrary([In] [MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/windows/desktop/ms683152%28v=vs.85%29.aspx</devdoc>
        [DllImport("kernel32", CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary([In] IntPtr hModule);
    }
}
