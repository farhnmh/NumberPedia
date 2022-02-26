import xml.etree.ElementTree as xml

def GenerateXML(x, y):
    hands = xml.Element("HandPosition")
    
    root = xml.Element("Hand")
    hands.append(root)

    posX = xml.SubElement(root, "PosX")
    posX.text = f"{x}"

    posY = xml.SubElement(root, "PosY")
    posY.text = f"{y}"

    tree = xml.ElementTree(hands)
    with open("HandsPosition.xml", "wb") as files:
        tree.write(files)

#if __name__ == "__main__":
#    GenerateXML("HandsPosition.xml")