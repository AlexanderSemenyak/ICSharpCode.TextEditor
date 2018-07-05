﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;

namespace ICSharpCode.TextEditor.Document
{
	public enum TextMarkerType
	{
		Invisible,
		SolidBlock,
		Underlined,
		WaveLine
	}
	
	/// <summary>
	/// Marks a part of a document.
	/// </summary>
    public class TextMarker : ISegment
    {
        [CLSCompliant(false)]
        protected int offset = -1;
        [CLSCompliant(false)]
        protected int length = -1;

        #region ICSharpCode.TextEditor.Document.ISegment interface implementation
        public int Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
        #endregion
        
	    public override string ToString()
        {
            return String.Format("[TextMarker: Offset = {0}, Length = {1}, Type = {2}]",
                                 offset,
                                 length,
                                 TextMarkerType);
        }

        public TextMarkerType TextMarkerType { get; }

        public Color Color { get; }

        public Color ForeColor { get; }

        public bool OverrideForeColor { get; } = false;

        /// <summary>
		/// Marks the text segment as read-only.
		/// </summary>
		public bool IsReadOnly { get; set; }
		
		public string ToolTip { get; set; } = null;

        /// <summary>
		/// Gets the last offset that is inside the marker region.
		/// </summary>
		public int EndOffset {
			get {
                return offset + length - 1;
			}
		}
		
		public TextMarker(int offset, int length, TextMarkerType textMarkerType) : this(offset, length, textMarkerType, Color.Red)
		{
		}
		
		public TextMarker(int offset, int length, TextMarkerType textMarkerType, Color color)
		{
			if (length < 1) length = 1;
			this.offset          = offset;
			this.length          = length;
			this.TextMarkerType  = textMarkerType;
			this.Color           = color;
		}
		
		public TextMarker(int offset, int length, TextMarkerType textMarkerType, Color color, Color foreColor)
		{
			if (length < 1) length = 1;
			this.offset          = offset;
			this.length          = length;
			this.TextMarkerType  = textMarkerType;
			this.Color           = color;
			this.ForeColor       = foreColor;
			OverrideForeColor = true;
		}
    }
}
