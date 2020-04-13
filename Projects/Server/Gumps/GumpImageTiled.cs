/***************************************************************************
 *                             GumpImageTiled.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using Server.Network;

namespace Server.Gumps
{
  public class GumpImageTiled : GumpEntry
  {
    private static readonly byte[] m_LayoutName = Gump.StringToBuffer("gumppictiled");
    private int m_GumpID;
    private int m_Width, m_Height;
    private int m_X, m_Y;

    public GumpImageTiled(int x, int y, int width, int height, int gumpID)
    {
      m_X = x;
      m_Y = y;
      m_Width = width;
      m_Height = height;
      m_GumpID = gumpID;
    }

    public int X
    {
      get => m_X;
      set => Delta(ref m_X, value);
    }

    public int Y
    {
      get => m_Y;
      set => Delta(ref m_Y, value);
    }

    public int Width
    {
      get => m_Width;
      set => Delta(ref m_Width, value);
    }

    public int Height
    {
      get => m_Height;
      set => Delta(ref m_Height, value);
    }

    public int GumpID
    {
      get => m_GumpID;
      set => Delta(ref m_GumpID, value);
    }

    public override string Compile(NetState ns) => $"{{ gumppictiled {m_X} {m_Y} {m_Width} {m_Height} {m_GumpID} }}";

    public override void AppendTo(NetState ns, IGumpWriter disp)
    {
      disp.AppendLayout(m_LayoutName);
      disp.AppendLayout(m_X);
      disp.AppendLayout(m_Y);
      disp.AppendLayout(m_Width);
      disp.AppendLayout(m_Height);
      disp.AppendLayout(m_GumpID);
    }
  }
}