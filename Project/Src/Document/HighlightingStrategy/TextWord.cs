﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision$</version>
// </file>

using System;
using System.Diagnostics;
using System.Drawing;

namespace ICSharpCode.TextEditor.Document
{
	public enum TextWordType {
		Word,
		Space,
		Tab
	}
	
	/// <summary>
	/// This class represents single words with color information, two special versions of a word are
	/// spaces and tabs.
	/// </summary>
	public class TextWord
	{
	    private HighlightColor  color;
	    private readonly LineSegment     line;
	    private readonly IDocument       document;

	    public sealed class SpaceTextWord : TextWord
		{
			public SpaceTextWord()
			{
				Length = 1;
			}
			
			public SpaceTextWord(HighlightColor color)
			{
				Length = 1;
				SyntaxColor = color;
			}
			
			public override Font GetFont(FontContainer fontContainer)
			{
				return null;
			}
			
			public override TextWordType Type {
				get {
					return TextWordType.Space;
				}
			}
			public override bool IsWhiteSpace {
				get {
					return true;
				}
			}
		}
		
		public sealed class TabTextWord : TextWord
		{
			public TabTextWord()
			{
				Length = 1;
			}
			public TabTextWord(HighlightColor color)
			{
				Length = 1;
				SyntaxColor = color;
			}
			
			public override Font GetFont(FontContainer fontContainer)
			{
				return null;
			}
			
			public override TextWordType Type {
				get {
					return TextWordType.Tab;
				}
			}
			public override bool IsWhiteSpace {
				get {
					return true;
				}
			}
		}

	    public static TextWord Space { get; } = new SpaceTextWord();

	    public static TextWord Tab { get; } = new TabTextWord();

	    public int Offset { get; }

	    public int Length { get; private set; }

	    /// <summary>
		/// Splits the <paramref name="word"/> into two parts: the part before <paramref name="pos"/> is assigned to
		/// the reference parameter <paramref name="word"/>, the part after <paramref name="pos"/> is returned.
		/// </summary>
		public static TextWord Split(ref TextWord word, int pos)
		{
			#if DEBUG
			if (word.Type != TextWordType.Word)
				throw new ArgumentException("word.Type must be Word");
			if (pos <= 0)
				throw new ArgumentOutOfRangeException("pos", pos, "pos must be > 0");
			if (pos >= word.Length)
				throw new ArgumentOutOfRangeException("pos", pos, "pos must be < word.Length");
			#endif
			TextWord after = new TextWord(word.document, word.line, word.Offset + pos, word.Length - pos, word.color, word.HasDefaultColor);
			word = new TextWord(word.document, word.line, word.Offset, pos, word.color, word.HasDefaultColor);
			return after;
		}
		
		public bool HasDefaultColor { get; }

	    public virtual TextWordType Type {
			get {
				return TextWordType.Word;
			}
		}
		
		public string Word {
			get {
				if (document == null) {
					return String.Empty;
				}
				return document.GetText(line.Offset + Offset, Length);
			}
		}
		
		public virtual Font GetFont(FontContainer fontContainer)
		{
			return color.GetFont(fontContainer);
		}
		
		public Color Color {
			get {
				if (color == null)
					return Color.Black;
				else
					return color.Color;
			}
		}
		
		public bool Bold {
			get {
				if (color == null)
					return false;
				else
					return color.Bold;
			}
		}
		
		public bool Italic {
			get {
				if (color == null)
					return false;
				else
					return color.Italic;
			}
		}
		
		public HighlightColor SyntaxColor {
			get {
				return color;
			}
			set {
				Debug.Assert(value != null);
				color = value;
			}
		}
		
		public virtual bool IsWhiteSpace {
			get {
				return false;
			}
		}
		
		protected TextWord()
		{
		}
		
		// TAB
		public TextWord(IDocument document, LineSegment line, int offset, int length, HighlightColor color, bool hasDefaultColor)
		{
			Debug.Assert(document != null);
			Debug.Assert(line != null);
			Debug.Assert(color != null);
			
			this.document = document;
			this.line  = line;
			this.Offset = offset;
			this.Length = length;
			this.color = color;
			this.HasDefaultColor = hasDefaultColor;
		}
		
		/// <summary>
		/// Converts a <see cref="TextWord"/> instance to string (for debug purposes)
		/// </summary>
		public override string ToString()
		{
			return "[TextWord: Word = " + Word + ", Color = " + Color + "]";
		}
	}
}
