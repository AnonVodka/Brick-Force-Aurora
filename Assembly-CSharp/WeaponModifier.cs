using System.Collections.Generic;
using UnityEngine;

public class WeaponModifier : MonoBehaviour
{
	private Dictionary<int, WpnMod> dic;

	private Dictionary<int, WpnModEx> dicEx;

	private static WeaponModifier _instance;

	public static WeaponModifier Instance
	{
		get
		{
			if (null == _instance)
			{
				_instance = (Object.FindObjectOfType(typeof(WeaponModifier)) as WeaponModifier);
				if (null == _instance)
				{
					Debug.LogError("ERROR, Fail to get the WeaponModifier Instance");
				}
			}
			return _instance;
		}
	}

	public WpnMod Get(int weaponBy)
	{
		if (dic.ContainsKey(weaponBy))
		{
			return dic[weaponBy];
		}
		return null;
	}

	public void UpdateWpnMod(int nSeq, float fReloadSpeed, float fDrawSpeed, float fRange, float fSpeedFactor, float fAtkPow, float fRigidity, float fRateOfFire, float fRecoilPitch, float fThrowForce, float fAccuracy, float fAccurateMin, float fAccurateMax, float fInaccurateMin, float fInaccurateMax, float fAccurateSpread, float fAccurateCenter, float fInaccurateSpread, float fInaccurateCenter, float fMoveInaccuracyFactor, float fZAccuracy, float fZAccurateMin, float fZAccurateMax, float fZInaccurateMin, float fZInaccurateMax, float fZAccurateSpread, float fZAccurateCenter, float fZInaccurateSpread, float fZInaccurateCenter, float fZMoveInaccuracyFactor, float fZFov, float fZCamSpeed, float fSlashSpeed, int maxAmmo, int maxMagazine, float fExplosionTime, float fEffectiveRange, float fRecoilYaw, float brokenRatio, float radius)
	{
		if (!dic.ContainsKey(nSeq))
		{
			dic.Add(nSeq, new WpnMod());
		}
		if (dic.ContainsKey(nSeq))
		{
			dic[nSeq].nSeq = nSeq;
			dic[nSeq].fReloadSpeed = ((!(fReloadSpeed < 0f)) ? fReloadSpeed : 75f);
			dic[nSeq].fDrawSpeed = ((!(fDrawSpeed < 0f)) ? fDrawSpeed : 75f);
			dic[nSeq].fRange = ((!(fRange < 0f)) ? fRange : 75f);
			dic[nSeq].fSpeedFactor = ((!(fSpeedFactor < 0f)) ? fSpeedFactor : 75f);
			dic[nSeq].fAtkPow = ((!(fAtkPow < 0f)) ? fAtkPow : 75f);
			dic[nSeq].fRigidity = ((!(fRigidity < 0f)) ? fRigidity : 75f);
			dic[nSeq].fRateOfFire = ((!(fRateOfFire < 0f)) ? fRateOfFire : 75f);
			dic[nSeq].fRecoilPitch = ((!(fRecoilPitch < 0f)) ? fRecoilPitch : 75f);
			dic[nSeq].fThrowForce = ((!(fThrowForce < 0f)) ? fThrowForce : 75f);
			dic[nSeq].fAccuracy = ((!(fAccuracy < 0f)) ? fAccuracy : 75f);
			dic[nSeq].fAccurateMin = ((!(fAccurateMin < 0f)) ? fAccurateMin : 75f);
			dic[nSeq].fAccurateMax = ((!(fAccurateMax < 0f)) ? fAccurateMax : 75f);
			dic[nSeq].fInaccurateMin = ((!(fInaccurateMin < 0f)) ? fInaccurateMin : 75f);
			dic[nSeq].fInaccurateMax = ((!(fInaccurateMax < 0f)) ? fInaccurateMax : 75f);
			dic[nSeq].fAccurateSpread = ((!(fAccurateSpread < 0f)) ? fAccurateSpread : 75f);
			dic[nSeq].fAccurateCenter = ((!(fAccurateCenter < 0f)) ? fAccurateCenter : 75f);
			dic[nSeq].fInaccurateSpread = ((!(fInaccurateSpread < 0f)) ? fInaccurateSpread : 75f);
			dic[nSeq].fInaccurateCenter = ((!(fInaccurateCenter < 0f)) ? fInaccurateCenter : 75f);
			dic[nSeq].fMoveInaccuracyFactor = ((!(fMoveInaccuracyFactor < 0f)) ? fMoveInaccuracyFactor : 75f);
			dic[nSeq].fZAccuracy = ((!(fZAccuracy < 0f)) ? fZAccuracy : 75f);
			dic[nSeq].fZAccurateMin = ((!(fZAccurateMin < 0f)) ? fZAccurateMin : 75f);
			dic[nSeq].fZAccurateMax = ((!(fZAccurateMax < 0f)) ? fZAccurateMax : 75f);
			dic[nSeq].fZInaccurateMin = ((!(fZInaccurateMin < 0f)) ? fZInaccurateMin : 75f);
			dic[nSeq].fZInaccurateMax = ((!(fZInaccurateMax < 0f)) ? fZInaccurateMax : 75f);
			dic[nSeq].fZAccurateSpread = ((!(fZAccurateSpread < 0f)) ? fZAccurateSpread : 75f);
			dic[nSeq].fZAccurateCenter = ((!(fZAccurateCenter < 0f)) ? fZAccurateCenter : 75f);
			dic[nSeq].fZInaccurateSpread = ((!(fZInaccurateSpread < 0f)) ? fZInaccurateSpread : 75f);
			dic[nSeq].fZInaccurateCenter = ((!(fZInaccurateCenter < 0f)) ? fZInaccurateCenter : 75f);
			dic[nSeq].fZMoveInaccuracyFactor = ((!(fZMoveInaccuracyFactor < 0f)) ? fZMoveInaccuracyFactor : 75f);
			dic[nSeq].fZFov = ((!(fZFov < 0f)) ? fZFov : 75f);
			dic[nSeq].fZCamSpeed = ((!(fZCamSpeed < 0f)) ? fZCamSpeed : 75f);
			dic[nSeq].fSlashSpeed = ((!(fSlashSpeed < 0f)) ? fSlashSpeed : 75f);
			dic[nSeq].maxAmmo = ((maxAmmo >= 0) ? maxAmmo : 10);
			dic[nSeq].maxMagazine = ((maxMagazine >= 0) ? maxMagazine : 10);
			dic[nSeq].explosionTime = ((!(fExplosionTime < 0f)) ? fExplosionTime : 75f);
			dic[nSeq].effectiveRange = ((!(fEffectiveRange < 0f)) ? Mathf.Min(fEffectiveRange, fRange) : 75f);
			dic[nSeq].recoilYaw = fRecoilYaw;
			dic[nSeq].brokenRatio = brokenRatio;
			dic[nSeq].radius = ((!(radius < 0f)) ? radius : 75f);
		}
	}

	public WpnModEx GetEx(int weaponBy)
	{
		if (dicEx.ContainsKey(weaponBy))
		{
			return dicEx[weaponBy];
		}
		return null;
	}

	public void UpdateWpnModEx(int nSeq, float misSpeed, float throwForce, int maxLauncherAmmo, float radius2ndWpn, int damage2ndWpn, float recoilPitch2ndWpn, float recoilYaw2ndWpn, float Radius1stWpn, int semiAutoMaxCyclicAmmo, int minBuckShot, int maxBuckShot, float persistTime, float continueTime)
	{
		if (!dicEx.ContainsKey(nSeq))
		{
			dicEx.Add(nSeq, new WpnModEx());
		}
		if (dicEx.ContainsKey(nSeq))
		{
			dicEx[nSeq].nSeq = nSeq;
			dicEx[nSeq].misSpeed = ((!(misSpeed < 0f)) ? misSpeed : 75f);
			dicEx[nSeq].throwForce = ((!(throwForce < 0f)) ? throwForce : 75f);
			dicEx[nSeq].maxLauncherAmmo = ((maxLauncherAmmo >= 0) ? maxLauncherAmmo : 0);
			dicEx[nSeq].radius2ndWpn = ((!(radius2ndWpn < 0f)) ? radius2ndWpn : 75f);
			dicEx[nSeq].damage2ndWpn = ((damage2ndWpn >= 0) ? damage2ndWpn : 0);
			dicEx[nSeq].recoilPitch2ndWpn = ((!(recoilPitch2ndWpn < 0f)) ? recoilPitch2ndWpn : 75f);
			dicEx[nSeq].recoilYaw2ndWpn = recoilYaw2ndWpn;
			dicEx[nSeq].Radius1stWpn = ((!(Radius1stWpn < 0f)) ? Radius1stWpn : 75f);
			dicEx[nSeq].semiAutoMaxCyclicAmmo = ((semiAutoMaxCyclicAmmo >= 0) ? semiAutoMaxCyclicAmmo : 0);
			dicEx[nSeq].minBuckShot = ((minBuckShot >= 0) ? minBuckShot : 0);
			dicEx[nSeq].maxBuckShot = ((maxBuckShot >= 0) ? maxBuckShot : 0);
			dicEx[nSeq].persistTime = ((!(persistTime < 0f)) ? persistTime : 75f);
			dicEx[nSeq].continueTime = ((!(continueTime < 0f)) ? continueTime : 75f);
		}
	}

	public void Awake()
	{
		dic = new Dictionary<int, WpnMod>();
		dicEx = new Dictionary<int, WpnModEx>();
		Object.DontDestroyOnLoad(this);
	}

	public void Clear()
	{
		dic.Clear();
		dicEx.Clear();
	}
}
