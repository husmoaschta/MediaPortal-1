using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D=Microsoft.DirectX.Direct3D;


namespace MediaPortal.GUI.Library
{
	/// <summary>
	/// Summary description for GUIImageList.
	/// </summary>
	public class GUIImageList:  GUIControl
	{
		[XMLSkinElement("align")]				Alignment		m_alignment=Alignment.ALIGN_LEFT;
		[XMLSkinElement("orientation")]	eOrientation m_orientation=eOrientation.Horizontal;
		[XMLSkinElement("textureWidth")]	int         m_textureWidth=32;
		[XMLSkinElement("textureHeight")]	int         m_textureHeight=32;
		[XMLSkinElement("percentage")]		string      m_strTag=String.Empty;
		int m_iPercentage;

		ArrayList		m_items=new ArrayList();

		public GUIImageList(int dwParentID) : base(dwParentID)
		{
			m_iPercentage=0;
		}


		public override void FinalizeConstruction()
		{
			base.FinalizeConstruction();
			for (int i=0; i < SubItemCount;++i)
			{
				string strTexture = (string)GetSubItem(i);
				GUIImage img= new GUIImage(m_dwParentID, m_dwControlID, m_dwPosX, m_dwPosY,m_textureWidth, m_textureHeight,strTexture,0);
				m_items.Add(img);
			}
		}


		public override void AllocResources()
		{
			foreach (GUIImage img in m_items)
			{
				img.AllocResources();
			}
		}
		public override void FreeResources()
		{
			foreach (GUIImage img in m_items)
			{
				img.FreeResources();
			}
		}

		public override void Render(float timePassed)
		{
			if (!IsVisible) return;
			if (m_strTag!=String.Empty)
			{
				string percent=GUIPropertyManager.Parse(m_strTag);
				try
				{
					Percentage=Int32.Parse(percent);
				}
				catch(Exception){}
			}
			if (	m_orientation==eOrientation.Horizontal)
			{
				RenderHorizontal(timePassed);
			}
			else
			{
				RenderVertical(timePassed);
			}
		}

		void RenderHorizontal(float timePassed)
		{
			int startx =m_dwPosX;
			int imagesToDraw=m_dwWidth/m_textureWidth;
			for (int i=0; i < imagesToDraw;++i)
			{
				int texture=0;
				int currentPercent = (i*100)/imagesToDraw;
				if (currentPercent<Percentage)
				{
					int textureCount=m_items.Count-1;
					float fcurrentPercent=currentPercent;
					fcurrentPercent/=100f;
					fcurrentPercent *= ((float)textureCount);
					texture = (int)fcurrentPercent;

					if (m_alignment==Alignment.ALIGN_RIGHT)
						texture=textureCount-texture;
					if (texture < 1) texture=1;
					if (texture >= m_items.Count) texture=m_items.Count-1;
				}

				GUIImage img = (GUIImage)m_items[texture];
				img.SetPosition(startx, m_dwPosY);
				img.Render(timePassed);
				if (m_alignment==Alignment.ALIGN_LEFT)
					startx += m_textureWidth;
				else
					startx -= m_textureWidth;
			}
		}
		
		void RenderVertical(float timePassed)
		{
		}

		public int Percentage
		{
			get { return m_iPercentage;}
			set { m_iPercentage=value;}
		}
	}
}
