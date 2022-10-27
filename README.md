# WinRTWriter

WinRTWriter is a proof-of-concept library to programmatically create WinRT components (WinMD files).

It has two output modes, one for outputting IDL (which you'd then feed to MIDL to get the WinMD), and one to output the WinMD directly.

The IDL part is fairly straightforward since it involves just a text template transformation; this is achieved via T4.

For the WinMD output, we use the ECMA-335 APIs; this is the same approach that C#/WinRT takes and in fact we could refactor the [C#/WinRT](https://github.com/microsoft/CsWinRT) code to extract common code and reuse it here.
