# -*- mode: python ; coding: utf-8 -*-


block_cipher = None

def get_mediapipe_path():
    import mediapipe
    mediapipe_path = mediapipe.__path__[0]
    return mediapipe_path

a = Analysis(['E:/Repositories-Git/NumberPedia-Unity/HandTracking/NumberPedia-HandTracking.py'],
             pathex=[],
             binaries=[],
             datas=[('E:/Repositories-Git/NumberPedia-Unity/HandTracking/GeneralAttribute.py', '.'), ('E:/Repositories-Git/NumberPedia-Unity/HandTracking/UDPDataSender.py', '.'), ('E:/Repositories-Git/NumberPedia-Unity/HandTracking/Assets', 'Assets/')],
             hiddenimports=[],
             hookspath=[],
             hooksconfig={},
             runtime_hooks=[],
             excludes=[],
             win_no_prefer_redirects=False,
             win_private_assemblies=False,
             cipher=block_cipher,
             noarchive=False)
pyz = PYZ(a.pure, a.zipped_data,
             cipher=block_cipher)

mediapipe_tree = Tree(get_mediapipe_path(), prefix='mediapipe', excludes=["*.pyc"])
a.datas += mediapipe_tree
a.binaries = filter(lambda x: 'mediapipe' not in x[0], a.binaries)

exe = EXE(pyz,
          a.scripts, 
          [],
          exclude_binaries=True,
          name='NumberPedia-HandTracking',
          debug=False,
          bootloader_ignore_signals=False,
          strip=False,
          upx=True,
          console=False,
          disable_windowed_traceback=False,
          target_arch=None,
          codesign_identity=None,
          entitlements_file=None , icon='E:\\Repositories-Git\\NumberPedia-Unity\\HandTracking\\Assets\\logo_game.ico')
coll = COLLECT(exe,
               a.binaries,
               a.zipfiles,
               a.datas, 
               strip=False,
               upx=True,
               upx_exclude=[],
               name='NumberPedia-HandTracking')
