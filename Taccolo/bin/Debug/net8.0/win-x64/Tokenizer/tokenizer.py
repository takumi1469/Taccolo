import sys
import MeCab
import json

def tokenize(text):
    tagger = MeCab.Tagger("-O wakati")
    return tagger.parse(text).strip()

if __name__ == "__main__":
    input_text = sys.argv[1]
    tokens = tokenize(input_text)
    print(json.dumps(tokens, ensure_ascii=False))