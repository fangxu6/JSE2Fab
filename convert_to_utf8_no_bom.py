import glob
import chardet

files = glob.glob("**/*.cs", recursive=True)
for f in files:
    with open(f, "rb") as rf:
        raw_data = rf.read()
        result = chardet.detect(raw_data)
        if result["encoding"] == "GB2312" or result["encoding"] == "ascii":
            content = raw_data.decode(result["encoding"])
            with open(f, "w", encoding="utf-8", newline="\n") as wf:
                wf.write(content)
            print(f"Converted: {f} ({result['encoding']})")
        else:
            print(f"Skipped: {f} ({result['encoding']})")
