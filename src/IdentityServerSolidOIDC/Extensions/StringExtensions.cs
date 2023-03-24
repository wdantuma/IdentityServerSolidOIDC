using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerSolidOIDC.Extensions;

internal static class StringExtensions
{
    public static bool IsUri(this string input)
    {
        if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
        {
            return false;
        }

        if (uri.IsFile && !input.StartsWith(Uri.UriSchemeFile + "://", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
