<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

  <xsl:template match="CAA | ABSTRACT | ESTREMI"/>


  <xsl:template match="/">
    <document>
      <xsl:apply-templates select="CIPER/LEGGI-SPEC"/>
    </document>
  </xsl:template>

  <xsl:template match="LEGGI-SPEC | COMUNE">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="COMMENTO">
    <xsl:apply-templates select="DESCRIZIONE"/>
  </xsl:template>

  <xsl:template match="ARTICOLO">
    <xsl:apply-templates select="*[name() != 'RIF']"/>
    <xsl:apply-templates select="RIF"/>
  </xsl:template>

  <xsl:template match="DESCRIZIONE">
    <xsl:choose>
      <xsl:when test="parent::COMMENTO">
        <level>
          <div class="heading">
            <h6>
              <bold>
                <xsl:apply-templates select="PARA" mode="commento"/>
              </bold>
            </h6>
          </div>
          <xsl:apply-templates select="following-sibling::TESTO-COMM"/>
        </level>
      </xsl:when>
      <xsl:when test="parent::ARTICOLO">
        <div class="titel">
          <h3>
            <bold>
              <xsl:apply-templates select="PARA" mode="articolo"/>
            </bold>
          </h3>
        </div>
      </xsl:when>
    </xsl:choose>
  </xsl:template>



  <xsl:template match="TESTO-COMM">
    <level>
      <xsl:apply-templates/>
    </level>
  </xsl:template>

  <xsl:template match="PARA" mode="articolo">
    <xsl:text>Art. </xsl:text>
    <xsl:value-of select="../../@testo"/>
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="PARA" mode="commento">
    <xsl:value-of select="../../@testo"/>
    <xsl:text> </xsl:text>
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template name="sumText">
    <xsl:param name="sum"/>
    <xsl:param name="currentPosition"/>
    <xsl:variable name="tempVariable">
      <xsl:variable name="textBatch" select="node()"/>
      <xsl:variable name="textVariable" select="node()[$currentPosition]"/>
      <xsl:choose>
        <xsl:when test="/node()[$currentPosition]/self::*">
          <xsl:value-of select="0"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="string-length(translate(node()[position()],'&#x20;&#x9;&#xD;&#xA;',''))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="newSum" select="$tempVariable + $sum"/>
    <xsl:choose>
      <xsl:when test="node()[$currentPosition]/following-sibling::node()">
        <xsl:call-template name="sumText">
          <xsl:with-param name="sum" select="$newSum"/>
          <xsl:with-param name="currentPosition" select="$currentPosition + 1"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$newSum"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="PARA">
    <xsl:choose>
      <xsl:when test="following-sibling::PARA[1]/POSTILLA">
        <!--Take the length of current text-->
        <xsl:variable name="sumText">
          <xsl:call-template name="sumText">
            <xsl:with-param name="currentPosition" select="1"/>
            <xsl:with-param name="sum" select="0"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <!--Add italic for subheading text-->
          <xsl:when test="$sumText &lt; 40 and $sumText != 0">
            <p>
              <italic>
                <xsl:apply-templates/>
              </italic>
            </p>
          </xsl:when>
          <xsl:otherwise>
            <p>
              <xsl:apply-templates/>
            </p>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <p>
          <xsl:apply-templates/>
        </p>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="TIPOG">
    <italic>
      <xsl:apply-templates/>
    </italic>
  </xsl:template>

  <xsl:template match="POSTILLA">
    <span class="POSTILLA">
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match="RIF">
    <div class="biblio">
      <p>
        <bold>
          <xsl:text>BIBLIOGRAFIA: </xsl:text>
        </bold>
        <xsl:apply-templates/>
      </p>
    </div>
  </xsl:template>

  <xsl:template match="RICH-LEGGE-ART | RICH-LEGGE-ART | RICH-LEGGE | RICH-LEGGE-SPEC | RICH-ALTRI-COD">
    <xsl:call-template name="createLink"/>
  </xsl:template>

  <xsl:template match="RICH-SITO">
    <a href="{@url}">
      <xsl:apply-templates/>
    </a>
  </xsl:template>

  <xsl:template match="RICH-LEGGE-ART[parent::RIF] | RICH-LEGGE-ART[parent::RIF] | RICH-LEGGE[parent::RIF] | RICH-LEGGE-SPEC[parent::RIF] | RICH-ALTRI-COD[parent::RIF]">
    <p>
      <xsl:call-template name="createLink"/>
    </p>
  </xsl:template>

  <xsl:template name="createLink">
    <xsl:variable name="attrCount" select="count(@*)"/>
    <a>
      <xsl:attribute name="href">
        <xsl:text>http://www.example.org/</xsl:text>
        <xsl:value-of select="substring-after(name(), '-')"/>
        <xsl:text>?</xsl:text>
        <xsl:for-each select="@*">
          <xsl:value-of select="name()"/>
          <xsl:text>=</xsl:text>
          <xsl:value-of select="."/>
          <xsl:if test="position() &lt; $attrCount">
            <xsl:text disable-output-escaping="yes">&amp;</xsl:text>
          </xsl:if>
        </xsl:for-each>
      </xsl:attribute>
      <span class="Hyperlink">
        <xsl:apply-templates select="node()"/>
      </span>
    </a>
  </xsl:template>


  <xsl:template match="text()[not(normalize-space(.))]" />
</xsl:stylesheet>