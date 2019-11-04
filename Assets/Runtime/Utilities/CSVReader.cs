using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Railek.Unibase.Utilities
{
  public static class CSVReader
  {
    public static List<List<string>> Parse(string src)
    {
      var rows = new List<List<string>>();
      var cols = new List<string>();
      var buffer = new StringBuilder();

      var mode = ParsingMode.OutQuote;
      var requireTrimLineHead = false;
      var isBlank = new Regex(@"\s");

      var len = src.Length;

      for (var i = 0; i < len; ++i)
      {
        var c = src[i];

        if (requireTrimLineHead)
        {
          if (isBlank.IsMatch(c.ToString()))
          {
            continue;
          }

          requireTrimLineHead = false;
        }

        if (i + 1 == len)
        {
          switch (mode)
          {
            case ParsingMode.InQuote:
            {
              if (c == '"')
              {
              }
              else
              {
                buffer.Append(c);
              }

              cols.Add(buffer.ToString());
              rows.Add(cols);
              return rows;
            }

            case ParsingMode.OutQuote:
            {
              if (c == ',')
              {
                cols.Add(buffer.ToString());
                cols.Add(string.Empty);
                rows.Add(cols);
                return rows;
              }

              if (cols.Count == 0 && string.Empty.Equals(c.ToString().Trim()))
              {
                return rows;
              }

              buffer.Append(c);
              cols.Add(buffer.ToString());
              rows.Add(cols);

              return rows;
            }
            case ParsingMode.None:
            {
              break;
            }
            default:
            {
              throw new ArgumentOutOfRangeException();
            }
          }
        }

        var n = src[i + 1];

        switch (mode)
        {
          case ParsingMode.OutQuote:
          {
            switch (c)
            {
              case '"':
              {
                mode = ParsingMode.InQuote;
                break;
              }
              case ',':
              {
                cols.Add(buffer.ToString());
                buffer.Remove(0, buffer.Length);
                break;
              }
              case '\r' when n == '\n':
              {
                cols.Add(buffer.ToString());
                rows.Add(cols);
                cols = new List<string>();
                buffer.Remove(0, buffer.Length);
                ++i;
                requireTrimLineHead = true;
                break;
              }
              case '\n':
              case '\r':
              {
                cols.Add(buffer.ToString());
                rows.Add(cols);
                cols = new List<string>();
                buffer.Remove(0, buffer.Length);
                requireTrimLineHead = true;
                break;
              }
              default:
              {
                buffer.Append(c);
                break;
              }
            }

            break;
          }
          case ParsingMode.InQuote:
          {
            switch (c)
            {
              case '"' when n != '"':
              {
                mode = ParsingMode.OutQuote;
                break;
              }
              case '"' when n == '"':
              {
                buffer.Append('"');
                ++i;
                break;
              }
              default:
              {
                buffer.Append(c);
                break;
              }
            }

            break;
          }
          case ParsingMode.None:
          {
            break;
          }
          default:
          {
            throw new ArgumentOutOfRangeException();
          }
        }
      }

      return rows;
    }

    private enum ParsingMode
    {
      None,
      OutQuote,
      InQuote
    }
  }
}
