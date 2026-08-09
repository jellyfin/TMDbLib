using System.ComponentModel;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Polyfill so that <c>record</c> and <c>init</c> accessors compile on netstandard2.0.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit;
