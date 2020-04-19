﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SpacerUnion
{
    public partial class ObjTree : Form
    {
        const string TAG_FOLDER = "folder";

        public ObjTree()
        {
            InitializeComponent();
            globalTree.DrawMode = TreeViewDrawMode.OwnerDrawText;
            globalTree.HideSelection = false;
        }

        public class TreeEntry
        {
            public int zCVob;
            public int parent;
            public string name;
            public TreeNode node;
            public string folderName;
            public string className;
            public bool isLevel;

        }

  
        public static int CreateAndGetFolder(string className)
        {
            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            int classNameFoundPos = -1;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Text == className)
                {
                    classNameFoundPos = i;
                    break;
                }
            }

            if (classNameFoundPos == -1)
            {

                TreeNode newNode = nodes.Add(className);
                newNode.Tag = TAG_FOLDER;
                newNode.ImageIndex = 0;
                newNode.SelectedImageIndex = 0;

                classNameFoundPos = newNode.Index;
            }

            return classNameFoundPos;
        }


        public class ParentResult
        {
            public TreeNode node;
            public bool isCompo;
            public TreeEntry parentEntry;
        }


        public static List<TreeEntry> globalEntries = new List<TreeEntry>();

        public static ParentResult GetParentResult(int parentId)
        {
            TreeEntry entry = globalEntries
                    .Where(pair => pair.zCVob == parentId)
                    .Select(pair => pair)
                    .First();

            ParentResult result = new ParentResult();


            if (entry != null)
            {
               // Console.WriteLine("Parent found " + entry.zCVob);
                result.isCompo = entry.isLevel;
                result.node = entry.node;
                result.parentEntry = entry;
            }

            return result;
        }

        public static int noParentCount = 0;

        public static void ApplyNodeImage(string className, TreeNode node, bool isNew=false)
        {
            if (isNew)
            {
                node.ImageIndex = 3;
                node.SelectedImageIndex = 3;
                return;
            }

            if (className == "zCVobWaypoint" || className == "zCVobSpot")
            {
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
            }
            else
            {
                node.ImageIndex = 1;
                node.SelectedImageIndex = 1;
            }
        }

        public static void AddVobToNodes(TreeEntry entry)
        {
            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            string className = entry.className;

            int classNameFoundPos = -1;

            classNameFoundPos = CreateAndGetFolder(className);

            // levelCompo или воб без родителя
            if (entry.isLevel || entry.parent == 0)
            {
                if (entry.isLevel)
                {
                   // Console.WriteLine("LevelCompo: " + entry.name + " Address: " + entry.zCVob + " Class: " + entry.className);
                }

               
                TreeNode node = nodes[classNameFoundPos].Nodes.Add(entry.name);
                node.Tag = entry.zCVob;
                
                entry.node = node;

                ApplyNodeImage(className, node);


            }
            else if (entry.parent != 0)
            {
                //Console.WriteLine("Try parent: " + entry.name + " Parent: " + entry.parent + " Class: " + entry.className);
                ParentResult parentResult = GetParentResult(entry.parent);

                if (parentResult != null)
                {
                    if (!parentResult.isCompo)
                    {
                        TreeNode parentNode = parentResult.node;

                        if (parentNode == null)
                        {
                            noParentCount++;
                            //Console.WriteLine("Parent node is null. ParentAddr: " + entry.parent 
                              //  + " Child: " + entry.name + " Parent: " + parentResult.parentEntry.name);
                            return;
                        }


                        string name = entry.name; //entry.zCVob + " " + entry.name + "[" + entry.parent + "]"
                                                  //Console.WriteLine("Parent ok");

                        TreeNode node = parentNode.Nodes.Add(name);
                        node.Tag = entry.zCVob;
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;
                        entry.node = node;
                        ApplyNodeImage(className, node);
                    }
                    else
                    {
                        string name = entry.name; //"!!! : " + entry.zCVob + " " + entry.name + "[" + entry.parent + "]"


                        TreeNode node = nodes[classNameFoundPos].Nodes.Add(name);
                        node.Tag = entry.zCVob;
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;
                        entry.node = node;
                        ApplyNodeImage(className, node);
                    }
                    
                   
                }
                else
                {
                    noParentCount++;
                    Console.WriteLine(noParentCount + " Parent " + entry.parent + " is null: " + entry.name);
                }
                /*
                else
                {
                    Console.WriteLine("Parent is null: " + entry.name);
                    string name = entry.name; //"!!! : " + entry.zCVob + " " + entry.name + "[" + entry.parent + "]"
                    TreeNode node = nodes[classNameFoundPos].Nodes.Add(name);
                    node.Tag = entry.zCVob;
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                    entry.node = node;
                }
                */
            }
            //Console.WriteLine("Entry Node: " + entry.node.Text);

        }

        public static void CreateTreeRecursive()
        {
            noParentCount = 0;
            for (int i = 0; i < globalEntries.Count; i++)
            {
                AddVobToNodes(globalEntries[i]);
            }

            Console.WriteLine("Не смог вставить вобов: " + noParentCount + "/" + globalEntries.Count);

            if (noParentCount > 0)
            {
                //CreateTreeRecursive();
            }

        }

        [DllExport]
        public static void CreateTree()
        {
            CreateTreeRecursive();
        }

        [DllExport]
        public static void OnSelectVob(int ptr)
        {
            Console.WriteLine("OnSelectVob: " + ptr);

            if (ptr == 0)
            {
                return;
            }



            //MessageBox.Show(ptr.ToString());
            for (int i = 0; i < globalEntries.Count; i++)
            {
                // Console.WriteLine("AddVobToNodes: " + i);

                

                if (globalEntries[i].zCVob == ptr)
                {
                    TreeNode node = globalEntries[i].node;
                    //Console.WriteLine(node.Text);

                    if (node == null)
                    {
                        Console.WriteLine("OnSelectVob addr " + ptr + ", node is null, index is " + i);
                    }
                    else
                    {
                        UnionNET.objTreeWin.globalTree.SelectedNode = globalEntries[i].node;
                        //UnionNET.objTreeWin.globalTree.SelectedNode.Expand();
                    }
                    
                   
                    break;
                }
            }
        }


        [DllExport]
        public static void OnVobRemove(int vob)
        {
            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

            Console.WriteLine("OnVobRemove: " + vob);


            if (vob == 0)
            {
                return;
            }


            List<TreeEntry> entries = globalEntries
                    .Where(pair => pair.zCVob == vob || pair.parent == vob)
                    .ToList();

            if (entries.Count > 0)
            {

                for (int i = 0; i < entries.Count; i++)
                {
                    if (entries[i].node != null)
                    {
                        Console.WriteLine("Remove node: " + entries[i].node.Text);
                        entries[i].node.Remove();
                    }
                    else
                    {
                        Console.WriteLine("Remove node... already is NULL");
                    }
                }
            }

            globalEntries = globalEntries
                    .Where(pair => pair.zCVob != vob || pair.parent != vob)
                    .ToList();



        }

        [DllExport]
        public static void OnVobInsert(IntPtr ptr, int vob, int parent, IntPtr classNamePtr)
        {
            string name = Marshal.PtrToStringAnsi(ptr);
            string className = Marshal.PtrToStringAnsi(classNamePtr);

            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;

          


            Console.WriteLine("OnVobInsert: " + name + " parent: " + parent);

            int classNameFoundPos = -1;

            classNameFoundPos = CreateAndGetFolder(className);

            TreeEntry entry = new TreeEntry();

            entry.name = name;
            entry.parent = parent;
            entry.zCVob = vob;
            entry.className = className;
            entry.isLevel = entry.className == "zCVobLevelCompo";
            globalEntries.Add(entry);

            if (parent == 0)
            {
                TreeNode node = nodes[classNameFoundPos].Nodes.Add(name);
                node.Tag = vob;
                entry.node = node;
                ApplyNodeImage(className, node, true);
                UnionNET.objTreeWin.globalTree.SelectedNode = node;
            }
            else
            {

            }
        }

        [DllExport]
        public static void AddTreeNode(IntPtr ptr, int vob, int parent, IntPtr classNamePtr)
        {
            string name = Marshal.PtrToStringAnsi(ptr);
            string className = Marshal.PtrToStringAnsi(classNamePtr);

            TreeNodeCollection nodes = UnionNET.objTreeWin.globalTree.Nodes;


            if (name.ToUpper().Contains("EDITOR_CAMERA_VOB"))
            {
                Console.WriteLine("Ignoring EDITOR_CAMERA_VOB");
                return;
            }


            TreeEntry entry = new TreeEntry();

            entry.name = name;
            entry.parent = parent;
            entry.zCVob = vob;
            entry.className = className;
            entry.isLevel = entry.className == "zCVobLevelCompo";
            globalEntries.Add(entry);

           // Console.WriteLine(name + " has classname: " + className);
            /*

            if (globalNodes.ContainsValue(parent))
            {
                TreeNode existNode = globalNodes.Where(pair => pair.Value == parent)
                    .Select(pair => pair.Key)
                    .First();

                if (existNode != null)
                {
                    existNode.Nodes.Add(name);
                }
                else
                {
                    Console.WriteLine("No parent found");
                }
            }
            else
            {
                node = nodes.Add(name);
                globalNodes.Add(node, parent);
            }
            */


        }


      

        private void globalTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null) return;

            // if treeview's HideSelection property is "True", 
            // this will always returns "False" on unfocused treeview
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;

            // we need to do owner drawing only on a selected node
            // and when the treeview is unfocused, else let the OS do it for us
            if (selected && unfocused)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            globalTree.CollapseAll();
        }

        private void buttonExpand_Click(object sender, EventArgs e)
        {
            globalTree.ExpandAll();
        }

        private void ObjTree_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }


        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_SelectVobSync(int a);

        [DllImport("SpacerUnionNet.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Extern_SelectVob(int a);


        private void globalTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        private void globalTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

            Console.WriteLine("OnSelectDoubleClick: " + tag);


            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                Console.WriteLine("No select for: " + tag);
                return;
            }

            int addr = Convert.ToInt32(node.Tag);
            
            Extern_SelectVob(addr);
            UnionNET.form.Focus();
        }

        private void globalTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = globalTree.SelectedNode;

            string tag = node.Tag.ToString();

            Console.WriteLine("OnSelectVobSync: " + tag);

            if (tag.Length == 0 || tag == TAG_FOLDER)
            {
                Console.WriteLine("No select for: " + tag);
                return;
            }

            int addr = Convert.ToInt32(node.Tag);

          
            Extern_SelectVobSync(addr);
            UnionNET.form.Focus();
        }
    }
}
