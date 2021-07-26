using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class UndoRedoStack
    {
        public readonly struct Item
        {
            public Action Undo { get; }
            public Action Redo { get; }

            public Item(Action undo, Action redo)
            {
                Undo = undo;
                Redo = redo;
            }
        }

        private const int Capacity = 10;
        private static List<Item> undoStack = new List<Item>(10);
        private static List<Item> redoStack = new List<Item>(10);

        private static int OldestIndex(List<Item> collection) => 0;
        private static int NewestIndex(List<Item> collection) => collection.Count - 1;

        private static Item RetrieveAndRemoveNewest(List<Item> collection)
        {
            int index = NewestIndex(collection);
            Item item = collection[index];
            collection.RemoveAt(index);
            return item;
        }
        
        private static void TryAction(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private static void Add(List<Item> collection, Item item)
        {
            if (collection.Count == Capacity)
            {
                collection.RemoveAt(OldestIndex(collection));
            }
            
            collection.Insert(NewestIndex(collection) + 1, item);
        }

        public static void Enqueue(Action undo, Action redo)
        {
            Add(undoStack, new Item(undo, redo));
        }
        
        public static void Undo()
        {
            if (undoStack.Count == 0)
            {
                return;
            }

            Item item = RetrieveAndRemoveNewest(undoStack);
            TryAction(item.Undo);
            Add(redoStack, item);
        }
        
        public static void Redo()
        {
            if (redoStack.Count == 0)
            {
                return;
            }

            Item item = RetrieveAndRemoveNewest(redoStack);
            TryAction(item.Redo);
            Add(undoStack, item);
        }
    }
}