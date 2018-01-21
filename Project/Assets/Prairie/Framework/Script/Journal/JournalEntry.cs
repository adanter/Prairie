﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Journal/JournalEntry", order = 1)]
public class JournalEntry : ScriptableObject
{
	public string title;
	public List<string> content;
	public List<string> imagePaths;

	public override bool Equals(System.Object obj)
	{
		if (obj == null)
			return false;
		JournalEntry e = obj as JournalEntry ;
		if ((System.Object) e == null)
			return false;
		return title.Equals(e.title);
	}

	public bool Equals(JournalEntry e)
	{
		if ((object) e == null)
			return false;
		return title.Equals(e.title);
	}
}

