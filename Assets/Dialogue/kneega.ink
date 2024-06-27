
-> main
=== main ===
Hey, What's Up do you want candies?
    + [What candies?!]
        -> secondChoicesHello
    + [Ignore]
        Eyo
        -> END
    + [Yes]
        ->kneega

=== secondChoicesHello ===
Chocobar
    + [Ask about it]
        It's yummy
        sdsdasd
        -Done
    +[can i have one?]
       yes
       -> END

        
=== kneega ===
GAME_OVER
    +[Okay]
        GAME_OVER
        ->END
    +[Where we going?]
        GAME_OVER
-> END
    