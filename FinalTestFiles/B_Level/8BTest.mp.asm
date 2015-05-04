BR L0
L0:
PUSH D0
ADD SP #2 SP
SUB SP #3 D0
PUSH #"This is test 9 for B level"
WRTS
PUSH #""
WRTLNS
PUSH #"This will test repeat until loops"
WRTS
PUSH #""
WRTLNS
PUSH #"This should not compile, why?"
WRTS
PUSH #""
WRTLNS
L1:
PUSH #"\nThis line should print once\n"
WRTS
PUSH #""
WRTLNS
PUSH #1
PUSH #2
