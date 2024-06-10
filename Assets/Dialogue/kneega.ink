
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
    + [Ask about t]
        No, Thanks
        -> END
    + [Say goodbye]
        
        -> END
        
=== kneega ===
GAME_OVER
    +[Okay]
        GAME_OVER
        ->END
    +[Where we going?]
        GAME_OVER
-> END
    