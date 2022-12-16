namespace HelseID.Samples.Configuration;

/// <summary>
/// This class contains configurations that correspond to existing clients in the HelseID TEST environment.
/// </summary>
public class HelseIdSamplesConfiguration
{
    private const string stsUrl = "https://helseid-sts.test.nhn.no";

    // --------------------------------------------------------------------------------------------------------------------

    // The corresponding public key in HelseID Test: {'e':'AQAB','kty':'RSA','n':'qcByQdQX3xXnvjdZwJfiCh87KNVSDdxD6wOuUlfAod1d_1-nGFzFj8mlWVIdUTRuZAoD9urHU-t7M9K4Ytg7u6hfxrESXVlXRpfAxDEKbDK_xt1V9AKqN1E6vCKXIXvkZMYgdstjBejjC1-3IjwApvxEb8XVzpFWfI_dZc4pyk6E7VSJZGYYNxJuSXCLntkHrGgmaWp9BAbfq1S__WwufHAc6JvebGAYgCH0dihV-ueNqbS6a2lHJUqc88baCSwRrPjEDLzmOaZIsNmWvO-TJtqWdYUmdGK7OPMwnRs8s-v4WtwtHsSODklbYC8Q6E1Wmf268NlFlMTz8x9QeKP9j8P16vI8a2Dz4Uuli1YbkUnPoEqLI_asV_fE5eo4aydaTzJK_8CNm5XB0M8XoFqTQe72AOEhhkAdROa_5SOLWdBc61Bx4U0_evv1ER-vS52e8q_6qPNFntrGKYzKWnqoNmYnO117ksjnKTn9uFj88U4Akxwcxyql0a9vYK9p_IaVbxIesZ0EzAanUgWkiOpMyjJxeo8PxAO3n1O1sQ92EIEglYJ5qLzgegWKoZZLxRD_3jUoDNKbTeelqE2Rr9725VzrGa8VIv7te2ezz3AszfsGcU2emKVaZmfo043dy4KwYq-NmJgAt84ODoqgydqIAjHh4QU3vyDcRhLVqcZEjCU','kid':'BE9A843ED0A783F7881977BA671A8533'}
    // --------------------------------------------------------------------------------------------------------------------
    // In a production scenario, this private key MUST be secured inside the client, and NOT be set in source code:
    private const string ClientCredentialsSampleRsaPrivateKeyJwk = "{'d':'n_3zcpn9WdTilETFAiHk-RdQgf71FH07QmE4xwFQXP8wngZAjlS6G_i5MIOVgDkDpqRN8kZ8UltKxqBgC0G0ov1iL_oqzfLrsGxNUlzKbwox-LQaVB7C4dpcmn-bpAKKVHTsmiq-DQ7gF5NCHzEgiEKSGisDhmszcbmCuXZjqvrwjGO2Dvuu7REhA3ThE3mIovWS5p5ocAgufeQmBCwXcE-W36IxNEIqNXJLX6ZKy2289muRnLUgSkSVw6nzTNvNWRMqbLsJ3uIb9xtN2IuRTPiPAPrbMKzxuw5zDlBimU3ZyYFlwW8OWObJCdGyhNCHxCICNQkDbOFo8Afep6Ygz_uzu7eBPDI6ualbGFFRjWWuXqakcOpHEari-DWAigLXDKQ990x7MgfeyHf_tPhTXVcvOfdMmBbtBks812NduoBx3_3XoqgHIKT1_yUAZarsEJHZF0AIYtIxvjokiq1fqQvy3UaWuYeTffgHYbziMW0Xz0J18bsfQxrFjMaB5FqJLzTqkXnZ6wpxSq3AyNoXNz5IJXLmg9A0IT0HPqM3-gVf8AY0YRL4DPKvLip6BJN3AKa5PQSuKBYFi52IbJXB0Xfx9ghtOVNZDGZYdo75_gCpMkZFKLs6SlnW8fBCejmHuQurDnhkDE1B3H2WWOyz_IFr6UaN1jJzioU-HAio5-0','dp':'sLpQ1TesYWE2wJ3C0yvpD0E7gzw-VNdQJCi8DE6V227TIKVQI9sqzBCGjPN8cxiObE_7NWz-TZbPOTkbQ33eVyIH8g6cOf8LlCN_mV0b7Tn5UX_LzhOpBxW1O7Z4txE-LW_pgfdT_FnrJiQSWKpFHTSrFpB4ZkkF0Hx-AKO2jqcKFSsv0g4OYlNBN2ok9hDqVl1dILC0uv6B2ppRDrO0g86wuTEUwF9eTa8onOy1uUJ6y6Tc1JwFTs8o57h5LlQ8y2-xGL1juiK8GhBLR5k_5ht6k73217u_JHQDEKsEZbZ3DpU_tbjVKht2Br5gw_4ZCyET8OtLEcidsJFIwGPfYQ','dq':'CuVhvtQfQlbxoS3r3UGYN2zm3J3Bpvvg8qBH91Nd8vHsT9mQyvSqchnlaBXjHvzU_vBDgiEOmCUYwMVuwS4PbJL57vOSet1F9gOOYh5gkm9wOSDZ0oeaby8ZlrNXkouvpM4Cl8tSXku-vHhh6rx1zwQvbmvCrAnRS267VFsAD2zoVv8Poy3kGLDLXkG70MqoBQ7efcJcOYqA3JEXwJX-pKoWAWF9j548RTpqxNdJvXuhil873H9JJRA34tddsHLwE-BnL-iBeqsTpSKoCrLJqg8zOE3etVvvnVeepXABjgdbhJxz36KcMJkU_d6Uc_W3TfZu6EIjJGo1uEgcRkyNMw','e':'AQAB','kty':'RSA','n':'qcByQdQX3xXnvjdZwJfiCh87KNVSDdxD6wOuUlfAod1d_1-nGFzFj8mlWVIdUTRuZAoD9urHU-t7M9K4Ytg7u6hfxrESXVlXRpfAxDEKbDK_xt1V9AKqN1E6vCKXIXvkZMYgdstjBejjC1-3IjwApvxEb8XVzpFWfI_dZc4pyk6E7VSJZGYYNxJuSXCLntkHrGgmaWp9BAbfq1S__WwufHAc6JvebGAYgCH0dihV-ueNqbS6a2lHJUqc88baCSwRrPjEDLzmOaZIsNmWvO-TJtqWdYUmdGK7OPMwnRs8s-v4WtwtHsSODklbYC8Q6E1Wmf268NlFlMTz8x9QeKP9j8P16vI8a2Dz4Uuli1YbkUnPoEqLI_asV_fE5eo4aydaTzJK_8CNm5XB0M8XoFqTQe72AOEhhkAdROa_5SOLWdBc61Bx4U0_evv1ER-vS52e8q_6qPNFntrGKYzKWnqoNmYnO117ksjnKTn9uFj88U4Akxwcxyql0a9vYK9p_IaVbxIesZ0EzAanUgWkiOpMyjJxeo8PxAO3n1O1sQ92EIEglYJ5qLzgegWKoZZLxRD_3jUoDNKbTeelqE2Rr9725VzrGa8VIv7te2ezz3AszfsGcU2emKVaZmfo043dy4KwYq-NmJgAt84ODoqgydqIAjHh4QU3vyDcRhLVqcZEjCU','p':'0KxlUgbxuakt1gfWmr7SAKNJmH_SnsMkEMrw-AFWnAcBB9yB7JHjsgyhmkfrz2QmGfgQjit7u1GtkclWZmXY_Bcq9-mhulB2wNNxtLZgIXT_Y4UK6X1W1Y-qLmXu8RCzwxvtVpEvZPGk73dLVMbMJ9JI0K0MSAgsZUJo7OUGD-3QA8m1c-yq1zK26rmnvYlWTToFKCKis4_Ql0PfbG3S8etugnsCXkHSAt3JmAuZJ72HL76nyHjZv-Q23M4xCEDYOOGBjT1O_zfRpBtUT95iJXegk9qPabBB-ZghFRjRqL-1V1uluUTBNnb15mm2ER2UJu_7XCsUIgmEbulQTF0hTw','q':'0EBA0XqE7K3My1VDDjgaWwQgqfdcG5aUKGgd1WVBf242odkTrH4gTymMIWTNhbUxLpsFoO3DUZPQW9emTr_qGR1g0z5L4mrSVMZRzNeDO80iAurLK8ft_d_0fOWEW-wnwRzY8Iog0hgd4QasZIRVXnDqUenVbif3bt48s_LkL0F0-xpAhZn2yTGAkFH6qfSQN3ZMWMGo8TKStPdJ9iBX_twjy7PFQLZ0Rq1apzyXKfwxuNWvzEYAeVhj-Hp6Rl_AATr76sY-gE2EiFjH4lkq_cAKHcuET1WOKxhDRUGMbtwOVPU1-9r091wr0fqmFAVSBkBraKxDrkbJA6VCGjEWSw','qi':'QAznSkYlpX6Mp7Fq9-tmT1fKzpC45IfEwCjdHaJtki17er2_ch-B4knqOV20jZOhHHhEP_Dy7_ytia68qjexWdJI8roca7LHrUmdtKZO4md5fUCnC989285RMiNBllAK-0XACu0oK9coinbAVq3nstbxBknwe7qoNG-BkcqHAE5uX4dkJkNZQwTQfS_Cgj6lD6u36t-VJc3QH4LHv_RrNiMUBfm-S1jpDm_q_VUZ7U7jUu7Ei5sS4ptkJk9uJreyytOWzXbT_WxpIHQkPOSvi1Px0zZAV4oCqXyVOA1uwZCxWKN3aO_dLT4H45vJg1Bd0DD8TvEzYpCKzRxLniXb0A','kid':'BE9A843ED0A783F7881977BA671A8533'}";        
    // --------------------------------------------------------------------------------------------------------------------
    
    private const string ClientCredentialsSampleClientId = "helseid-sample-client-credentials";
    private const string ClientCredentialsSampleScope = "nhn:helseid-public-samplecode/client-credentials";

    // --------------------------------------------------------------------------------------------------------------------
    // In a production scenario, this private key MUST be secured inside the client, and NOT be set in source code:
    private const string ClientCredentialsWithUnderenhetSamplePrivateKeyJwk = "{'d':'JrIh4aNzYjG74j7aB-8LOHOBnlOwthXsxwaI5BwoyiUc6kVdgeeZq2XOlkyqWBbF5pzb-2WITkydJ7KK2KzbhKtf6mQOYE7edALzuCIscEoR7nxxByIYFvcL8vtYcahNGe-Xz1SJVYpljArWPfzfCFXIPIO11qW5PRfYpraMoJrwVQiJEVWkiZKZO6AZepqR_Tyuk6N6r-g7aqgnoPetDYw6CL651lxdynxuu4dzmL0GsfvzjKdEAMHF0dod3lGSM0Y7DHcN78WHsY6yzBuKSWHES4eqj5RU6UBhaAxSg3U_HGy7QybIkvb5-NILZtD6yYC-KTusazWfNKmqF2uP6A_M_JA5yOusaJFPf4cUPDMfL_u7TUIkwlro11pw049woUrb1jk5652-ieNYZj4eoCm-EEh18_WvJCdkSgY4nIldkPWV7d_QpCkKb8daLa7jLIfJyjBY0rABeUy5NgKlUU2LOBNzJttUBSMtdBxqbdEkgzIyvGUXYWzeZSWq7RRCdFI_Y6oLY9AEr_3yJM-8-dhjiq3nybXSLhx3LBkNyil6q_Ef_JO1Ki4FE4m0Q3ngCFv7vWKRg9HCt13_Z4LZO_peOaFG08g4j6R--m_P9fG09OJcRczT9NKeWnERoqFMlFc_AOyq6TE3GuwlHoJVduc6k-3eI84oOs2DTD-Ogdk','dp':'YzLd4BTcTdT4ed26rGZXTp1GhJNI2eryEuyCN9mAYaNFCYgs5jgfXvuB6NuBbblVgg46vBK882BmInQZmQcLSZDV4uLtwLmF8WBQe7RQ2kAPb3vvu3F-jVfqR1kf7E9iDKPgbyUBG6OHzdXMXk-e4T23xdfr3jjtAS5V_YFqTMk1aiLYT34OPVcsZm9M5EC03tTauz14Pd--eyAVOzOsulIZJdBZdlpF2ChDzMc1TQXtcdbymmtcbvPZvMopc6x9Aw7bCpUDa7dLSHYsJREDrss3OQ8-GOlLQeA3yaTq5A_PCBHEwS5FdBgOwlOcSEooiy4xrJw3YTJZnpL1m91tvQ','dq':'k6UF0D_N2peglEfC7bmpo__IX7LeVKKBzFsnh-Lmo6MrHQQyVWY7-7dq2tNZKvd4QxiFDaeHYF7vbgnrqqtdaGUUdS8nVhvWbpvngWLAZh_o3EZ3N_PY8x_ZAIMn33_rXpYHuxg4mjnca22khqDD-xTCpVYQTxDVFOSsNrpU_hpoRgbfwha5U2HZdQ-GdRRAKHq4GvDWuvswnqlDbH31l8hzOcFU-PWwGwo0xTUN4OfK9K1CJ3OvXCS0chbllqyZYI9E2jI-Nde8faUpM23VYF5V2Z9HKbtaFUakqQlaR9uXtIxT2EVO-OEMRqlbPDNnYyu9v-QbthBLWQgX2MDclQ','e':'AQAB','kty':'RSA','n':'oMV8mrf9wS2z6sBrF9_aNculRp5RHC8tGanalfmnGeZ47icC8R6Z93zFJ44EKrvFqYKv_ndxPrE25HAWg3O3tRFf8YXnVmRoCzBC6h8KInJa0-1tBWgETTy2UdlM2HH5WZBvo8I1uFPLAJ95Ha9jw7STZhS_J11yM8Gko3KqQ3ERbgQPF1EURhimKJg_iFvd-vrWCVEFilpaeeg2ayqmmMU1_oTtSgI1qu4R-ztJliFsTEFlcUsWTKF9gsYv2dHaO2fv8cCdeC9hdT8SlE7pt0s7xu_3jG4KzUnSPZG_Z3D2-KEeaWS21aJnq0ic-7BwFkJGVk3tWcfi0w8jTMqHrTBFkZhja7HBw5ZsFoNhJ4c1Kt0vUdF6Y9_sUOQY8I7Tj6Yim6lq7VlL9zpTO7isl5bErafbtq0JxH9gXKeroGz1TMw1JqiP8RCZYsZNoHWx1-GS-21ktOD72jFrK2i3M2LVYXdaSc0OqDFoYCB31NriC309iZmfvqC5z84FFKki0r_oUrg11GQf9esbGPJeryIg7x_taKtl72e7WpimuGsdlC5Y6_cxYtJ2iTuwtSxOgNpbEy6-k3jBls_UBHDxFTFehUpKvfoQ9qGer905TeLnvGKTSuFOqmyXDbjlh-d57jFUkbZ3VCo6Im-npqpAYWLoALow_NanLQRZAoFvCuk','p':'xlcDWaJ4FAD8ifGiM5X_se9DkmwqZxeKJPJHWyRRn-bPUWgQu3tlikb0PaYDmi166S1CC3INIzPy9rkxomCZFUKVs7-WT5BWDxNWNU1vEHSKRElospSAY6UmV4f364A2s8XOseT6nbT0v5w8M4P0Eqe87FzsKdZRtdR5viv0LVN54Srtih3xH_RUZJ-KME9sCqKea8_6txmqtGcIqeSKATWOwKvxHpEVDzcHnX22Vje4lR0inAe9GdqMTlGuGhHX-jgvzyPtMu1zcn33XybideyVjN3s-ctEdP3Pf4NyzP1iZBHIQkDgMzcFaFiWTA7wf-N77-0JdhY75StA_JJXxw','q':'z4KHJkY9b4K2iC0wh25mppNEPPqkUyVEyKee8_fd3a-vXQ1b8dxdx8dhBWc-1GE2kvB8anze8EGdn8GZRhoy5vfYtC46mLTBs50U0HUwXziCPxZzkUAB-IQXPFu1IW7hocbfXMrMyp4VWjDF7Y-DPmdakGRBxtgWmz7jjCFmWPUFoplV1h0dqy9-TC9dhTxi2LI7QesgByAojGxc1cRvBJcDpYFkmdK-_bfFq7SEoLw4J5LDlKWpGldtKwb4dx-n4WQih4LJIhTw2Bi4gxi4dH8u5UUO0VDNHGMut-evuHE5U-B3qfd0D2qGG9q43mePxqhJDyx0wHr7_-Ma9e9nzw','qi':'k4j15FTmUivfDPRdS4uQ-02enadgBzUHPXDrlM-SC5SVfhrXSeN7a426yKKc_Mdt-GSFT1QKYOZYfCfX9eXvidJIP1recl2nvegyRfynXcN07O2fCpqRXhNyffNt1sf7Q9LHpVTpY6AzgSAqXFTsx3PyGwJNGoznB5crFMHjt1wK2F8hhyEM_8u8UP0KeYuD5H8eIVq5V00yMK3xh-i1iL2FWFgBypJ4EsQZq6D15EwJX9LZCA5UfbQ_sifspudumbyTz6Y7DfKeVMdCr5c7YGD1Svxgm7ZfUwnG3HFutvl9ZTGVcVLMsEw0pclxxQhdKkPLFk4HxyCUnNnkJJgazw','kid':'EF5F04B8B1FCACD24F7A907A6E2FACC1'}";
    // --------------------------------------------------------------------------------------------------------------------

    private const string ClientCredentialsWithUnderenhetSampleClientId = "helseid-sample-client-credentials-with-underenhet";
    private const string ClientCredentialsWithUnderenhetSampleClientIdScope = "nhn:helseid-public-samplecode/client-credentials";
    
    // The underenhet (child organization) number must match a number in the client's whitelist:
    public const string ClientCredentialsWithUnderenhetOrgNo = "999977775";

    // --------------------------------------------------------------------------------------------------------------------
    // In a production scenario, this private key MUST be secured inside the client, and NOT be set in source code:
    private const string ClientInfoSampleRsaPrivateKeyJwk = "{  'p': '6O-Qui3cqSwYluQRkvJ0uXa38NNl0MzrXLaWWHD9vQOsQ_jVSwL9TM0xQj-AA8gMMEtzbULfqjfaoiuZ2TjTGiXMqjRmHeHjVDcLA39MgwNFjjCwAzOxxEZ7hdV81Ii6Dxquvu_BDJMHbxjZyYekKGHn1Upei9KACjMvjVC3qpk', 'kty': 'RSA', 'q': 'pbLYf3gWIIHlpKK7-h4-UWb_jrvOffpods_A2NrKnVM777JifzlvwxNue0raNMo0ch5yF6XUuoiaLvLH6Dh_GQ-JXSJ0nEFvUDlx6s9xDX_yeWOeA6v7G6RaKo5myiNpFoskfSC7Yj-Blf4IEZX1ENq18wrupTT78o6e0mhvVK0', 'd': 'MfvTgYUNOWXBztmtG0Ud6U0cA02fx2VPRjnYC3KwRvj6w4tZ-MKO7FkuFc5_sAHP04pNI2VzcmE2YjPwLiS74XEBhr2gHPUNvFYRMl8_b518mboG-dWd0HjBlD_Dfq6DUy-w-g1GZJeXHHGUjBnWLbmxJ-UXy7vWcT4sT3fzOzW7igBMgN-zxIdd-4wZrMHMwRWrdDFtNuMipqi8qBGYMLIFXneutzMpMd0YwiDOQGtFQEmiH0U3nqMRYacKecT_FT5eBJ77-uoq3gcbH7ZiRZYb9Ivikpsc1kwbSDRkLYwb1ISLxR5uM6U-Mea4UWgx9BnU1yvEFX4Ruei9k6dd4Q', 'e': 'AQAB', 'use': 'sig', 'kid': '/aZZYfn4F1RmjT8ssydGnK9/QRw=', 'qi': 'fjSxi_ve1ZLsQE-gAE5IDuKBzNABUvKq5kkwK3uF91dbskUhIjwfP29OBQppZyOv_98JZubHy7_08x9MSNHZD7IZ9lo5vQxeNDDU-nvbxOcGZHbsKpgnTy-obfx_wKN4ViY4nMKnpfMKQrXvwJX55GhozMdJokge7IwzrITq3J4', 'dp': 'dCn8qAx1DdzSynUkmn7VXSRqaOxTy0RWX98irSp0L83kG-W9IPJ1tdZiqWIXiks6YN9Pyf5eonnGS7eout6O0GxnW75T6rUa9IWatXzHgFKiXl3DeWVPUs2_jifAYBFrkFrDKK9SO94bB_mBqvI9GHJy9jhnXB13Ax8xqKzHW4k', 'alg': 'RS256', 'dq': 'ltZO5QLhSahV72BAtHiRjDKx0zI90EqCjB2lVQMezMa3WgVOSrhzd-aZfVzvdHzZ70St4b8Q_tlZWgGiX1AGyz5scj7qXk_mz-XrQLCkHoDprv0zG-6UAV7Ewdat1bcUc_QoPEvuqIpdIbiFidSzqSsf1OaPxg6MiAqyo6F0L2U', 'n': 'lsUj2L89bYfU-ilbOFXKgxTV1cRYhdDUmWwbulOVFLd_q51Tt2zzIeBJUqNY_-9offUM2enz9MpvAi7-UOjxGNwiKp6Ob0PLFswFhPi6Vv1mCiPx9BtIyDFSjHzYW1y3l7BmGtHyeNeF-uT-hf73Z8SCJTxhzhu29fTrSzYUF1jQ4nuWoGa2W-TJxs6OH71Sp-wsODlU99oqbjzu1AcKC24ro4xO_Sn3BDj66B_y51EpV63pBj9hKAiC_DGdEU95_TWMFOVEjvhcl3DeCKDcqbqTO-8AdUBZwQSIFe6I9NbS8QOhCYBZNsxkCPpRsmsiZ9gVAKCXckiMFH1u8G99ZQ' }";
    // --------------------------------------------------------------------------------------------------------------------
    
    private const string ClientInfoSampleClientId = "helseid-sample-client-info";
    
    private const string ClientInfoSampleScope = "helseid://scopes/client/info";
        
    public HelseIdSamplesConfiguration(
        string rsaPrivateKeyJwk,
        string clientId,
        string scope)
    {
        RsaPrivateKeyJwk = rsaPrivateKeyJwk;
        ClientId = clientId;
        Scope = scope;
    }
    // TODO: create a reference to the documentation for this: 
    /// <summary>
    /// The private key MUST be secured inside the client.
    /// </summary>
    public string RsaPrivateKeyJwk  { get; }

    public string ClientId { get; }
    
    public string Scope { get; }

    public string StsUrl => stsUrl;

    public static HelseIdSamplesConfiguration ConfigurationForClientCredentialsClient =>
        new HelseIdSamplesConfiguration(
            ClientCredentialsSampleRsaPrivateKeyJwk,
            ClientCredentialsSampleClientId,
            ClientCredentialsSampleScope);

    public static HelseIdSamplesConfiguration ConfigurationForClientCredentialsWithUnderenhetClient =>
        new HelseIdSamplesConfiguration(
            ClientCredentialsWithUnderenhetSamplePrivateKeyJwk,
            ClientCredentialsWithUnderenhetSampleClientId,
            ClientCredentialsWithUnderenhetSampleClientIdScope);

    public static HelseIdSamplesConfiguration ConfigurationForClientInfoClient =>
        new HelseIdSamplesConfiguration(
            ClientInfoSampleRsaPrivateKeyJwk,
            ClientInfoSampleClientId,
            ClientInfoSampleScope);

}
