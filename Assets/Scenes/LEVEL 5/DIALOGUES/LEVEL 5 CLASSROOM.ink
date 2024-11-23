Why are you late for class, Aris?
+[Tell the truth]
->truth
+[Sorry, I'm late]
->sorry

==truth==
Teddy had returned, he came back. He escaped from the bad guys.
The students began whispers and chatting.
Quiet, children. That's a wonderful news then.
->discuss
==sorry==
Sorry, I'm late.
It's alright.
->discuss
==discuss==
Today, we will talk about making the right choices.
I'll be telling scenarios and you will tell me what you would do.
Let's begin.
A man in a van stop by and asked you.
"Come in to my van, I have a lot of ice cream and candy at the back." What would you do?
+[Go with the man.]
->INCORRECT1
+[Don't go]
->RIGHT

==INCORRECT1==
No, you don't know him and that's not safe.
->q2
==RIGHT==
That's right!
->q2

==q2==
There's a woman who said that she is your mom's co-worker.
"I'm your mom's friend and she ask me to pick you up." What would you do?
+[Go with her.]
->INCORRECT2
+[Leave her.]
->RIGHT2
==INCORRECT2==
Are you sure? She can be lying.
->q3
==RIGHT2==
Yes, you can't be sure if she's telling the truth.
->q3
==q3==
If you neighbor said, come with me. There's an emergency!
Your mom is at the hospital! She had an accident!
+[I don't believe you]
->RIGHT3
+[Okay, I'm going!]
->INCORRECT3
==RIGHT3==
Right, don't believe until it's proven.
->ending
==INCORRECT3==
It's worrying, but we can't be sure.
->ending

==ending==
Alright, that's it for now.
->END
