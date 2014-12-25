﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;

namespace WorkflowActivities
{
    public sealed class WorkflowStep<T> : NativeActivity<T>
    {
        // Define an activity input argument of type string
        public InArgument<string> BookmarkName { get; set; }

        public OutArgument<T> Input { get; set; }

        public WorkflowStep()
            : base()
        {
        }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(NativeActivityContext context)
        {
            string bookmarkName = context.GetValue(this.BookmarkName);

            context.CreateBookmark(bookmarkName, new BookmarkCallback(this.Continue));

            Console.Out.WriteLine(bookmarkName);
        }

        void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            Input.Set(context, (T)obj);
        }

        protected override bool CanInduceIdle { get { return true; } }
    }
}
