using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BinarySearchTree : MonoBehaviour
{
    public class TreeNode
    {
        public int Value;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(int value)
        {
            Value = value;
        }
    }

    private TreeNode rootNode = null;

    public void Add(int data)
    {
        if (rootNode == null)
        {
            rootNode = new TreeNode(data);
            return;
        }
        TreeNode node = rootNode;

        while (node != null)
        {
            if (node.Value == data)
            {
                throw new
                    ApplicationException("중복입니다");
            }
            else
            {
                if (data < node.Value)
                {


                }
                else
                {

                }
            }

        }
    }




    public bool Search(int data)
    {
        TreeNode node = rootNode;

        while (node != null)
        {
            if (node.Value == data)
            {
                return true;

            }
            else
            {
                if (node.Value < data)
                {
                    node = node.Left;

                }
                else
                {
                    node = node.Right;
                }
            }
        }

        return false;

    }

    public void Remove(int data)
    {
        //case1
        //삭제노드 자식없음

        TreeNode node = rootNode;
        TreeNode prevNode = null;

        /*while(Node != null)
        {
            if(Node.Value == data)
            {
                break;
            }
            else
            {
                if (Node.Value < data)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }
        }

        if(node.Left != null && node.Right == null)
        {

            if(prevNode.Left == node)
            {
                prevNode.Left = null;

            }
            else
            {
                prevNode.Right = null;
            }
            node = null;
        }
        else if(node.Left == null || node.Right == null)
        {

        }
        else
        {

        }


        //case2
        //삭제 노드의 자식이 1개


        //case3
        //자식2개이상


    }*/


    }
}
