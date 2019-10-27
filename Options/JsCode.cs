namespace MCWrapper.CLI.Tests.Options
{
    /// <summary>
    /// Please note that you CANNOT have any whitespace trailing the closing semi-colon prior to the "}" bracket else multichain-cli.exe will return a javascript parsing error
    /// </summary>
    public struct JsCode
    {
        /// <summary>
        /// Unescaped Transaction Filter used for testing purposes
        /// </summary>
        public const string DummyTxFilterCodeEscapedForWindowsCLI = "\"function filtertransaction() { var tx=getfiltertransaction(); if (tx.vout.length > 100) return 'One output required';}\"";

        /// <summary>
        /// Unescaped Stream Filter used for testing purposes
        /// </summary>
        public const string DummyStreamFilterCodeEscapedForWindowsCLI = "\"function filterstreamitem() { var item=getfilterstreamitem(); if (item.keys.length > 100) return 'At least two keys required';}\"";
    }
}
